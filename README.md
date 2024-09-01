# ArduBoy

This is a project to make my own gameboy-like console, with its own scripting language and dynamic runtime on an Arduino.

Physically, its more or less equal to a Gameboy Pocket, except this one is a bit thicker and the screen is a bit smaller.
Other than that, it has the same button layout. 
Instead of using disposable batteries, it has a internal LiPo instead with a USB-C port for charging.
Games are semi-compiled into a easier to read format for the Arduino. The semi-compiled code is put on an SD card, that the ArduBoy can then parse and run.

In hindsight, it might have a bit of an overkill battery of 3000mAh, so it can technically run for a few days without a charge.

You can take a look in the [Games](./Games) folder for some already made games for the Arduino, or you can make some yourself!

You can use the CLI tool to compile the script file onto an SD card.
The tool will also do a bit of syntax checking to make sure there arent obvious errors in the script file.

## ArduBoy Script (`.abs`) Syntax

Generally the syntax of the ArduBoy Scripts are heavy encapsulation with `(` and `)`.
An example of this can be seen below:

```
(
	(:name Example)
	(:includes (:include core))

	(:func Setup
		(
			// Setup code goes here
			// This code only runs once
		)
	)

	(:func Loop
		(
			// Looping code goes here
			// The code in here will loop
		)
	)
)
```

## Parts

Here is a general list of the electronic parts that was used in this project.

* [3000 mAh battery](https://www.ebay.com/itm/121823784057)
* [USB-C Charging board](https://www.ebay.com/itm/325273541405)
* [SD card reader](https://www.ebay.com/itm/170895501953)
* [Arduino Nano](https://www.ebay.com/itm/364546971433?var=634055018693)
* [Button PCB](https://www.ebay.com/itm/296243650365?var=594299882127)
* [OLED Screen](https://www.ebay.com/itm/225810919466)
* [3.7v to 5v boster](https://www.ebay.com/itm/255301455926)
