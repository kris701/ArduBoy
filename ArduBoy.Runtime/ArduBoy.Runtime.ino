#include <SPI.h>
#include <SD.h>

#define SDPin 4

File gameFile;

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

    }
}
