#include <gfxfont.h>
#include <Adafruit_SPITFT_Macros.h>
#include <Adafruit_SPITFT.h>
#include <Adafruit_GrayOLED.h>
#include <Adafruit_GFX.h>
#include <Adafruit_SSD1331.h>
#include <SPI.h>
#include <SD.h>

#define SDPin                   10
#define SCREEN_CS               3
#define SCREEN_DC               2
#define INPUT_UP                8
#define INPUT_DOWN              7
#define INPUT_LEFT              A0
#define INPUT_RIGHT             A1
#define INPUT_A                 A2
#define INPUT_B                 A3
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

Adafruit_SSD1331 display = Adafruit_SSD1331(&SPI, SCREEN_CS, SCREEN_DC);
File gameFile;
int registers[32];
int stackPointers[32];
int currentStackPointer = -1;
const int colors[8] PROGMEM = {
    0x0000, // BLACK
    0x001F, // BLUE
    0xF800, // RED
    0x07E0, // GREEN
    0x07FF, // CYAN
    0xF81F, // MAGENTA
    0xFFE0, // YELLOW
    0xFFFF, // WHITE
};
int inputBuffer[32];

void setup() {
    if (!SD.begin(SDPin)) {
        Serial.println(F("initialization failed!"));
        while (1) { delay(1000); };
    }
    display.begin();
    display.fillScreen(colors[2]);

    pinMode(INPUT_UP, INPUT);
    pinMode(INPUT_DOWN, INPUT);
    pinMode(INPUT_LEFT, INPUT);
    pinMode(INPUT_RIGHT, INPUT);
    pinMode(INPUT_A, INPUT);
    pinMode(INPUT_B, INPUT);
    pinMode(INPUT_SELECT, INPUT);
    pinMode(INPUT_START, INPUT);

    gameFile = SD.open(F("game.rbs"), FILE_READ);
}

void loop() {
    delay(1000);
    return;


    while (gameFile.available()) {
        String line = gameFile.readStringUntil('\n');
        SplitString(&line, ' ');
        int target = line.charAt(0);
        switch (target)
        {
        case OP_CALL: DoCall(&line); break;
        case OP_IF: DoIf(&line); break;
        case OP_WAIT: DoWait(&line); break;
        case OP_SET: DoSet(&line); break;
        case OP_ADD: DoAdd(&line); break;
        case OP_SUB: DoSub(&line); break;
        case OP_MULT: DoMult(&line); break;
        case OP_DIV: DoDiv(&line); break;
        case OP_AUDIO: DoAudio(&line); break;
        case OP_DRAW_LINE: DoDrawLine(&line); break;
        case OP_DRAW_CIRCLE: DoDrawCircle(&line, false); break;
        case OP_DRAW_FILL_CIRCLE: DoDrawCircle(&line, true); break;
        case OP_DRAW_TRIANGLE: DoDrawTriangle(&line, false); break;
        case OP_DRAW_FILL_TRIANGLE: DoDrawTriangle(&line, true); break;
        case OP_DRAW_RECTANGLE: DoDrawRectangle(&line, false); break;
        case OP_DRAW_FILL_RECTANGLE: DoDrawRectangle(&line, true); break;
        case OP_DRAW_TEXT: DoDrawText(&line); break;
        case OP_GOTO: DoGoto(&line); break;
        case OP_FUNC_END: gameFile.seek(stackPointers[currentStackPointer--]); break;
        default:
            break;
        }
    }
}

void DoCall(String* str) {
    int pos = GetValue(str->substring(2));
    stackPointers[currentStackPointer++] = gameFile.position();
    gameFile.seek(pos);
}

void DoIf(String* str) {
    int skip = GetValue(str->substring(inputBuffer[0], inputBuffer[1]));
    String leftStr = str->substring(inputBuffer[1], inputBuffer[2]);
    int op = GetValue(str->substring(inputBuffer[2], inputBuffer[3]));
    String rightStr = str->substring(inputBuffer[3]);

    int left = GetValue(leftStr);
    int right = GetValue(rightStr);

    bool result = false;
    switch (op)
    {
    case 0x90: result = left == right; break;
    case 0x91: result = left > right; break;
    case 0x92: result = left < right; break;
    case 0x93: result = left != right; break;
    default:
        break;
    }

    if (!result)
        gameFile.seek(gameFile.position() + skip);
}

void DoWait(String* str) {
    int time = GetValue(str->substring(2));
    delay(time);
}

void DoSet(String* str) {
    int index = GetValue(str->substring(inputBuffer[0], inputBuffer[1]));
    int value = GetValue(str->substring(inputBuffer[1]));
    registers[index] = value;
}

void DoAdd(String* str) {
    int index = GetValue(str->substring(inputBuffer[0], inputBuffer[1]));
    int value = GetValue(str->substring(inputBuffer[1]));
    registers[index] += value;
}

void DoSub(String* str) {
    int index = GetValue(str->substring(inputBuffer[0], inputBuffer[1]));
    int value = GetValue(str->substring(inputBuffer[1]));
    registers[index] -= value;
}

void DoMult(String* str) {
    int index = GetValue(str->substring(inputBuffer[0], inputBuffer[1]));
    int value = GetValue(str->substring(inputBuffer[1]));
    registers[index] *= value;
}

void DoDiv(String* str) {
    int index = GetValue(str->substring(inputBuffer[0], inputBuffer[1]));
    int value = GetValue(str->substring(inputBuffer[1]));
    registers[index] /= value;
}

int GetValue(String str) {
    int value = -1;
    if (str.startsWith(F("%_")))
        value = GetReservedValue(str.substring(2).toInt());
    else if (str.startsWith(F("%")))
        value = registers[str.substring(1).toInt()];
    else
        value = str.toInt();
    return value;
}

int GetReservedValue(int target) {
    switch (target) {
    case INPUT_UP: return digitalRead(INPUT_UP);
    case INPUT_DOWN: return digitalRead(INPUT_DOWN);
    case INPUT_LEFT: return digitalRead(INPUT_LEFT);
    case INPUT_RIGHT: return digitalRead(INPUT_RIGHT);
    case INPUT_A: return digitalRead(INPUT_A);
    case INPUT_B: return digitalRead(INPUT_B);
    case INPUT_SELECT: return digitalRead(INPUT_SELECT);
    case INPUT_START: return digitalRead(INPUT_START);
    }
    return -1;
}

void SplitString(String* str, char delimiter) {
    inputBuffer[0] = str->indexOf(delimiter);
    int offset = inputBuffer[0];
    int index = 1;
    while (offset != -1) {
        offset = str->indexOf(delimiter, offset);
        if (offset != -1)
            inputBuffer[index++] = offset;
    }
}

void DoAudio(String* str) {
    int value = GetValue(str->substring(2));
    // Implement later
}

void DoDrawLine(String* str) {
    int x1 = GetValue(str->substring(inputBuffer[0], inputBuffer[1]));
    int y1 = GetValue(str->substring(inputBuffer[1], inputBuffer[2]));
    int x2 = GetValue(str->substring(inputBuffer[2], inputBuffer[3]));
    int y2 = GetValue(str->substring(inputBuffer[3], inputBuffer[4]));
    int color = colors[GetValue(str->substring(inputBuffer[4]))];
    display.drawLine(x1, y1, x2, y2, color);
}

void DoDrawCircle(String* str, bool fill) {
    int x = GetValue(str->substring(inputBuffer[0], inputBuffer[1]));
    int y = GetValue(str->substring(inputBuffer[1], inputBuffer[2]));
    int radius = GetValue(str->substring(inputBuffer[2], inputBuffer[3]));
    int color = colors[GetValue(str->substring(inputBuffer[3]))];
    if (fill)
        display.fillCircle(x, y, radius, color);
    else
        display.drawCircle(x, y, radius, color);
}

void DoDrawTriangle(String* str, bool fill) {
    int x1 = GetValue(str->substring(inputBuffer[0], inputBuffer[1]));
    int y1 = GetValue(str->substring(inputBuffer[1], inputBuffer[2]));
    int x2 = GetValue(str->substring(inputBuffer[2], inputBuffer[3]));
    int y2 = GetValue(str->substring(inputBuffer[3], inputBuffer[4]));
    int x3 = GetValue(str->substring(inputBuffer[4], inputBuffer[5]));
    int y3 = GetValue(str->substring(inputBuffer[5], inputBuffer[6]));
    int color = colors[GetValue(str->substring(inputBuffer[3]))];
    if (fill)
        display.fillTriangle(x1, y1, x2, y2, x3, y3, color);
    else
        display.drawTriangle(x1, y1, x2, y2, x3, y3, color);
}

void DoDrawRectangle(String* str, bool fill ) {
    int x = GetValue(str->substring(inputBuffer[0], inputBuffer[1]));
    int y = GetValue(str->substring(inputBuffer[1], inputBuffer[2]));
    int width = GetValue(str->substring(inputBuffer[2], inputBuffer[3]));
    int heigth = GetValue(str->substring(inputBuffer[3], inputBuffer[4]));
    int color = colors[GetValue(str->substring(inputBuffer[4]))];
    if (fill)
        display.fillRect(x, y, width, heigth, color);
    else
        display.drawRect(x, y, width, heigth, color);
}

void DoDrawText(String* str) {
    int x = GetValue(str->substring(inputBuffer[0], inputBuffer[1]));
    int y = GetValue(str->substring(inputBuffer[1], inputBuffer[2]));
    int size = GetValue(str->substring(inputBuffer[2], inputBuffer[3]));
    int color = colors[GetValue(str->substring(inputBuffer[3], inputBuffer[4]))];
    int text = GetValue(str->substring(inputBuffer[4]));
    display.setTextColor(color);
    display.setTextSize(size);
    display.setCursor(x, y);
    display.print(text);
}

void DoDrawFill(String* str) {
    display.fillScreen(colors[GetValue(str->substring(2))]);
}

void DoGoto(String* str) {
    int index = GetValue(str->substring(2));
    gameFile.seek(index);
}
