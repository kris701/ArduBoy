#include <SPI.h>
#include <SD.h>

#define SDPin 4

File gameFile;
int registers[32];
int stackPointers[32];
int currentStackPointer = 0;

void setup() {
    if (!SD.begin(SDPin)) {
        Serial.println(F("initialization failed!"));
        while (1) { delay(1000); };
    }
    gameFile = SD.open(F("game.rbs"), FILE_READ);
}

void loop() {
    while (gameFile.available()) {
        String line = gameFile.readStringUntil('\n');
        if (line.startsWith("1")) 
        {
            int pos = GetValue(line.substring(2));
            stackPointers[currentStackPointer++] = gameFile.position();
            gameFile.seek(pos);
            continue;
        }
        else if (line.startsWith("2"))
        {
            int indexes[4];
            SplitString(line, ' ', indexes);

            int skip = GetValue(line.substring(indexes[0], indexes[1]));
            String leftStr = line.substring(indexes[1], indexes[2]);
            int op = GetValue(line.substring(indexes[2], indexes[3]));
            String rightStr = line.substring(indexes[3]);

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
                gameFile.seek(skip);
        }
        else if (line.startsWith("3"))
        {
            int time = GetValue(line.substring(2));
            delay(time);
        }
        else if (line.startsWith("4"))
        {
            int indexes[2];
            SplitString(line, ' ', indexes);

            int index = GetValue(line.substring(indexes[0], indexes[1]));
            int value = GetValue(line.substring(indexes[1]));
            registers[index] = value;
        }
        else if (line.startsWith("5"))
        {
            SetAudio(GetValue(line.substring(2)));
        }
        else if (line.startsWith("6"))
        {
        }
        else if (line.startsWith("7"))
        {
            GoTo(GetValue(line.substring(2)));
        }
        else if (line == "") 
        {
            gameFile.seek(stackPointers[currentStackPointer--]);
            continue;
        };
    }
}

int GetValue(String str) {
    int value = -1;
    if (str.startsWith("%_"))
        value = GetReservedValue(str.substring(2).toInt());
    else if (str.startsWith("%"))
        value = registers[str.substring(1).toInt()];
    else
        value = str.toInt();
    return value;
}

int GetReservedValue(int target) {
    // Get input values
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

void SetAudio(int value) {
    // implement later
}

void DrawLine(int x1, int y1, int x2, int y2, int color) {
    // implement later
}

void GoTo(int index) {
    gameFile.seek(index);
};