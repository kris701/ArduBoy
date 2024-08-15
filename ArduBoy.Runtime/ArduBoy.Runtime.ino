#include <Adafruit_SSD1351.h>
#include <SPI.h>
#include <SD.h>

#define DEBUG

#define Audio_Pin               9
#define SD_CS                   10
#define SCREEN_CS               8
#define SCREEN_DC               7
#define SCREEN_RST              6
#define SCREEN_WIDTH            128
#define SCREEN_HEIGHT           128
#define INPUT_UP                A0
#define INPUT_DOWN              A1
#define INPUT_LEFT              A2
#define INPUT_RIGHT             A3
#define INPUT_A                 A4
#define INPUT_B                 A5
#define INPUT_SELECT            A6
#define INPUT_START             A7

#define BYTE_OFFSET             33
#define OP_FUNC_END             0 + BYTE_OFFSET
#define OP_CALL                 1 + BYTE_OFFSET
#define OP_IF                   2 + BYTE_OFFSET
#define OP_WAIT                 3 + BYTE_OFFSET
#define OP_SET                  4 + BYTE_OFFSET
#define OP_AUDIO                5 + BYTE_OFFSET
#define OP_DRAW_LINE            6 + BYTE_OFFSET
#define OP_GOTO                 7 + BYTE_OFFSET
#define OP_ADD                  8 + BYTE_OFFSET
#define OP_SUB                  9 + BYTE_OFFSET
#define OP_MULT                 10 + BYTE_OFFSET
#define OP_DIV                  11 + BYTE_OFFSET

#define OP_DRAW_CIRCLE          12 + BYTE_OFFSET
#define OP_DRAW_FILL_CIRCLE     13 + BYTE_OFFSET
#define OP_DRAW_TRIANGLE        14 + BYTE_OFFSET
#define OP_DRAW_FILL_TRIANGLE   15 + BYTE_OFFSET
#define OP_DRAW_RECTANGLE       16 + BYTE_OFFSET
#define OP_DRAW_FILL_RECTANGLE  17 + BYTE_OFFSET
#define OP_DRAW_TEXT            18 + BYTE_OFFSET
#define OP_DRAW_FILL            19 + BYTE_OFFSET

#define OP_IF_EQU               20 + BYTE_OFFSET
#define OP_IF_LES               21 + BYTE_OFFSET
#define OP_IF_LAR               22 + BYTE_OFFSET
#define OP_IF_NEQ               23 + BYTE_OFFSET

Adafruit_SSD1351 display = Adafruit_SSD1351(SCREEN_WIDTH, SCREEN_HEIGHT, &SPI, SCREEN_CS, SCREEN_DC, SCREEN_RST);
File gameFile;
int registers[32];
int stackPointers[32];
uint8_t currentStackPointer = 0;
enum mathExp {
    ADD, SUB,
    DIV, MUL,
    SET
};
const uint16_t  colors[8] = {
    0x0000, // BLACK
    0x001F, // BLUE
    0xF800, // RED
    0x07E0, // GREEN
    0x07FF, // CYAN
    0xF81F, // MAGENTA
    0xFFE0, // YELLOW
    0xFFFF, // WHITE
};
uint8_t inputBuffer[10];

void setup() {
#if defined(DEBUG)
    Serial.begin(115200);
#endif

    display.begin();
    display.fillScreen(colors[0]);
    display.setTextSize(2);
    display.setTextColor(colors[7]);

    for (int i = -20; i < 60; i += 3) {
        display.fillRect(0, i - 5, SCREEN_WIDTH, 16, colors[0]);
        display.setCursor(25, i);
        display.print(F("ArduBoy"));
        delay(25);
    }
    display.setTextSize(1);

    delay(1000);

    pinMode(INPUT_UP, INPUT);
    pinMode(INPUT_DOWN, INPUT);
    pinMode(INPUT_LEFT, INPUT);
    pinMode(INPUT_RIGHT, INPUT);
    pinMode(INPUT_A, INPUT);
    pinMode(INPUT_B, INPUT);
    pinMode(INPUT_SELECT, INPUT);
    pinMode(INPUT_START, INPUT);

    if (!SD.begin(SD_CS) || !SD.exists(F("game.bin"))) {
        while (1) { delay(1000); };
    }

    gameFile = SD.open(F("game.bin"), FILE_READ);
    display.fillScreen(colors[0]);
}

void loop() {
    while (gameFile.available()) {
        String line = gameFile.readStringUntil('\n');
        if (line == "")
            continue;
        SplitString(&line);
        int target = line.charAt(0);
        switch (target)
        {
        case OP_CALL: DoCall(&line); break;
        case OP_IF: DoIf(&line); break;
        case OP_WAIT: DoWait(&line); break;
        case OP_SET: DoMathExp(&line, mathExp::SET); break;
        case OP_ADD: DoMathExp(&line, mathExp::ADD); break;
        case OP_SUB: DoMathExp(&line, mathExp::SUB); break;
        case OP_MULT: DoMathExp(&line, mathExp::MUL); break;
        case OP_DIV: DoMathExp(&line, mathExp::DIV); break;
        case OP_AUDIO: DoAudio(&line); break;
        case OP_DRAW_LINE: DoDrawLine(&line); break;
        case OP_DRAW_CIRCLE: DoDrawCircle(&line, false); break;
        case OP_DRAW_FILL_CIRCLE: DoDrawCircle(&line, true); break;
        case OP_DRAW_TRIANGLE: DoDrawTriangle(&line, false); break;
        case OP_DRAW_FILL_TRIANGLE: DoDrawTriangle(&line, true); break;
        case OP_DRAW_RECTANGLE: DoDrawRectangle(&line, false); break;
        case OP_DRAW_FILL_RECTANGLE: DoDrawRectangle(&line, true); break;
        case OP_DRAW_TEXT: DoDrawText(&line); break;
        case OP_DRAW_FILL: DoDrawFill(&line); break;
        case OP_GOTO: DoGoto(&line); break;
        case OP_FUNC_END: 
            currentStackPointer--;
            gameFile.seek(stackPointers[currentStackPointer]); break;
        default:
            break;
        }
    }
}

int GetValueAsInt(const char* str, uint8_t from)
{
    if (str[from] == '%')
        return registers[atoi(str + from + 1)];
    if (str[from] == '|')
        return GetReservedValue(atoi(str + from + 1));
    return atoi(str + from);
}

String GetValueAsStr(String* str) {
    if (str->startsWith(F("%")) || str->startsWith(F("|")))
        return String(GetValueAsInt(str->c_str(), 0));
    return *str;
}

int GetReservedValue(int target) {
    switch (target) {
        // Input
    case 0: return digitalRead(INPUT_UP);
    case 1: return digitalRead(INPUT_DOWN);
    case 2: return digitalRead(INPUT_LEFT);
    case 3: return digitalRead(INPUT_RIGHT);
    case 4: return digitalRead(INPUT_A);
    case 5: return digitalRead(INPUT_B);
    case 6: return digitalRead(INPUT_SELECT);
    case 7: return digitalRead(INPUT_START);
        // Misc
    case 8: return millis();
    }
    return -1;
}

void SplitString(String* str) {
    inputBuffer[0] = str->indexOf(' ') + 1;
    uint8_t offset = inputBuffer[0];
    uint8_t index = 1;
    while (offset != 255) {
        offset = str->indexOf(' ', offset + 1);
        if (offset != 255)
            inputBuffer[index++] = offset + 1;
    }
}

#pragma region Actions

void DoCall(String* str) {
    int pos = GetValueAsInt(str->c_str(), 2);
    stackPointers[currentStackPointer++] = gameFile.position();
    gameFile.seek(pos - 1);
}

void DoIf(String* str) {
    int skip = GetValueAsInt(str->c_str(), inputBuffer[0]);
    int left = GetValueAsInt(str->c_str(), inputBuffer[1]);
    String opStr = str->substring(inputBuffer[2], inputBuffer[3]);
    opStr.trim();
    char op = opStr.charAt(0);
    int right = GetValueAsInt(str->c_str(), inputBuffer[3]);

    bool result = false;
    switch (op)
    {
    case OP_IF_EQU: result = left == right; break;
    case OP_IF_LAR: result = left > right; break;
    case OP_IF_LES: result = left < right; break;
    case OP_IF_NEQ: result = left != right; break;
    default:
        break;
    }

    if (!result)
        gameFile.seek(skip - 1);
}

void DoWait(String* str) {
    int time = GetValueAsInt(str->c_str(), 2);
    delay(time);
}

void DoMathExp(String* str, mathExp type) {
    int index = GetValueAsInt(str->c_str(), inputBuffer[0]);
    int value = GetValueAsInt(str->c_str(), inputBuffer[1]);
    switch (type)
    {
    case ADD: registers[index] += value; break;
    case SUB: registers[index] -= value; break;
    case DIV: registers[index] /= value; break;
    case MUL: registers[index] *= value; break;
    case SET: registers[index] = value; break;
    default:
        break;
    }
}

void DoAudio(String* str) {
    int value = GetValueAsInt(str->c_str(), 2);
    tone(Audio_Pin, value);
}

void DoDrawLine(String* str) {
    int x1 = GetValueAsInt(str->c_str(), inputBuffer[0]);
    int y1 = GetValueAsInt(str->c_str(), inputBuffer[1]);
    int x2 = GetValueAsInt(str->c_str(), inputBuffer[2]);
    int y2 = GetValueAsInt(str->c_str(), inputBuffer[3]);
    int color = colors[GetValueAsInt(str->c_str(), inputBuffer[4])];

    display.drawLine(x1, y1, x2, y2, color);
}

void DoDrawCircle(String* str, bool fill) {
    int x = GetValueAsInt(str->c_str(), inputBuffer[0]);
    int y = GetValueAsInt(str->c_str(), inputBuffer[1]);
    int radius = GetValueAsInt(str->c_str(), inputBuffer[2]);
    int color = colors[GetValueAsInt(str->c_str(), inputBuffer[3])];
    if (fill)
        display.fillCircle(x, y, radius, color);
    else
        display.drawCircle(x, y, radius, color);
}

void DoDrawTriangle(String* str, bool fill) {
    int x1 = GetValueAsInt(str->c_str(), inputBuffer[0]);
    int y1 = GetValueAsInt(str->c_str(), inputBuffer[1]);
    int x2 = GetValueAsInt(str->c_str(), inputBuffer[2]);
    int y2 = GetValueAsInt(str->c_str(), inputBuffer[3]);
    int x3 = GetValueAsInt(str->c_str(), inputBuffer[4]);
    int y3 = GetValueAsInt(str->c_str(), inputBuffer[5]);
    int color = colors[GetValueAsInt(str->c_str(), inputBuffer[6])];
    if (fill)
        display.fillTriangle(x1, y1, x2, y2, x3, y3, color);
    else
        display.drawTriangle(x1, y1, x2, y2, x3, y3, color);
}

void DoDrawRectangle(String* str, bool fill) {
    int x = GetValueAsInt(str->c_str(), inputBuffer[0]);
    int y = GetValueAsInt(str->c_str(), inputBuffer[1]);
    int width = GetValueAsInt(str->c_str(), inputBuffer[2]);
    int heigth = GetValueAsInt(str->c_str(), inputBuffer[3]);
    int color = colors[GetValueAsInt(str->c_str(), inputBuffer[4])];
    if (fill)
        display.fillRect(x, y, width, heigth, color);
    else
        display.drawRect(x, y, width, heigth, color);
}

void DoDrawText(String* str) {
    int x = GetValueAsInt(str->c_str(), inputBuffer[0]);
    int y = GetValueAsInt(str->c_str(), inputBuffer[1]);
    int size = GetValueAsInt(str->c_str(), inputBuffer[2]);
    int color = colors[GetValueAsInt(str->c_str(), inputBuffer[3])];
    String text = GetValueAsStr(&str->substring(inputBuffer[4]));

    display.setTextColor(color);
    display.setTextSize(size);
    display.setCursor(x, y);
    display.print(text);
}

void DoDrawFill(String* str) {
    display.fillScreen(colors[GetValueAsInt(str->c_str(), 2)]);
}

void DoGoto(String* str) {
    int index = GetValueAsInt(str->c_str(), 2);
    gameFile.seek(index - 1);
}

#pragma endregion
