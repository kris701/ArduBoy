#include <gfxfont.h>
#include <Adafruit_SPITFT_Macros.h>
#include <Adafruit_SPITFT.h>
#include <Adafruit_GrayOLED.h>
#include <Adafruit_GFX.h>
#include <Adafruit_SSD1331.h>
#include <SPI.h>
#include <SD.h>

#define SDPin 4
#define SCREEN_SCLK 13
#define SCREEN_MOSI 11
#define SCREEN_CS   10
#define SCREEN_RST  9
#define SCREEN_DC   8

#define OP_CALL "1"
#define OP_IF "2"
#define OP_WAIT "3"
#define OP_SET "4"
#define OP_AUDIO "5"
#define OP_DRAW_LINE "6"
#define OP_GOTO "7"
#define OP_ADD "8"
#define OP_SUB "9"
#define OP_MULT "10"
#define OP_DIV "11"

#define INPUT_UP 0
#define INPUT_DOWN 1
#define INPUT_LEFT 2
#define INPUT_RIGHT 3
#define INPUT_A 4
#define INPUT_B 5
#define INPUT_SELECT 6
#define INPUT_START 7

Adafruit_SSD1331 display = Adafruit_SSD1331(&SPI, SCREEN_CS, SCREEN_DC, SCREEN_RST);
File gameFile;
int registers[32];
int stackPointers[32];
int currentStackPointer = 0;
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

void setup() {
    if (!SD.begin(SDPin)) {
        Serial.println(F("initialization failed!"));
        while (1) { delay(1000); };
    }
    display.begin();
    display.fillScreen(colors[0]);

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
    while (gameFile.available()) {
        String line = gameFile.readStringUntil('\n');
        if (line.startsWith(OP_CALL))
            DoCall(line);
        else if (line.startsWith(OP_IF))
            DoIf(line);
        else if (line.startsWith(OP_WAIT))
            DoWait(line);
        else if (line.startsWith(OP_SET))
            DoSet(line);
        else if (line.startsWith(OP_ADD))
            DoAdd(line);
        else if (line.startsWith(OP_SUB))
            DoSub(line);
        else if (line.startsWith(OP_MULT))
            DoMult(line);
        else if (line.startsWith(OP_DIV))
            DoDiv(line);
        else if (line.startsWith(OP_AUDIO))
            DoAudio(line);
        else if (line.startsWith(OP_DRAW_LINE))
            DoDrawLine(line);
        else if (line.startsWith(OP_GOTO))
            DoGoto(line);
        else if (line == "-")
            gameFile.seek(stackPointers[currentStackPointer--]);
    }
}

void DoCall(String str) {
    int pos = GetValue(str.substring(2));
    stackPointers[currentStackPointer++] = gameFile.position();
    gameFile.seek(pos);
}

void DoIf(String str) {
    int indexes[4];
    SplitString(str, ' ', indexes);

    int skip = GetValue(str.substring(indexes[0], indexes[1]));
    String leftStr = str.substring(indexes[1], indexes[2]);
    int op = GetValue(str.substring(indexes[2], indexes[3]));
    String rightStr = str.substring(indexes[3]);

    int left = GetValue(leftStr);
    int right = GetValue(rightStr);

    bool evaluated = false;
    switch (op)
    {
    case 0x90: evaluated = left == right; break;
    case 0x91: evaluated = left > right; break;
    case 0x92: evaluated = left < right; break;
    case 0x93: evaluated = left != right; break;
    default:
        break;
    }

    if (!evaluated)
        gameFile.seek(gameFile.position() + skip);
}

void DoWait(String str) {
    int time = GetValue(str.substring(2));
    delay(time);
}

void DoSet(String str) {
    int indexes[2];
    SplitString(str, ' ', indexes);

    int index = GetValue(str.substring(indexes[0], indexes[1]));
    int value = GetValue(str.substring(indexes[1]));
    registers[index] = value;
}

void DoAdd(String str) {
    int indexes[2];
    SplitString(str, ' ', indexes);

    int index = GetValue(str.substring(indexes[0], indexes[1]));
    int value = GetValue(str.substring(indexes[1]));
    registers[index] += value;
}

void DoSub(String str) {
    int indexes[2];
    SplitString(str, ' ', indexes);

    int index = GetValue(str.substring(indexes[0], indexes[1]));
    int value = GetValue(str.substring(indexes[1]));
    registers[index] -= value;
}

void DoMult(String str) {
    int indexes[2];
    SplitString(str, ' ', indexes);

    int index = GetValue(str.substring(indexes[0], indexes[1]));
    int value = GetValue(str.substring(indexes[1]));
    registers[index] *= value;
}

void DoDiv(String str) {
    int indexes[2];
    SplitString(str, ' ', indexes);

    int index = GetValue(str.substring(indexes[0], indexes[1]));
    int value = GetValue(str.substring(indexes[1]));
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

void SplitString(String str, char delimiter, int indexes[]) {
    indexes[0] = str.indexOf(delimiter);
    int offset = indexes[0];
    int index = 1;
    while (offset != -1) {
        offset = str.indexOf(delimiter, offset);
        if (offset != -1)
            indexes[index++] = offset;
    }
}

void DoAudio(String str) {
    int value = GetValue(str.substring(2));
    // Implement later
}

void DoDrawLine(String str) {
    int indexes[5];
    SplitString(str, ' ', indexes);

    int x1 = GetValue(str.substring(indexes[0], indexes[1]));
    int y1 = GetValue(str.substring(indexes[1], indexes[2]));
    int x2 = GetValue(str.substring(indexes[2], indexes[3]));
    int y2 = GetValue(str.substring(indexes[3], indexes[4]));
    int color = colors[GetValue(str.substring(indexes[4]))];
    display.drawLine(x1, y1, x2, y2, color);
}

void DoGoto(String str) {
    int index = GetValue(str.substring(2));
    gameFile.seek(index);
}
