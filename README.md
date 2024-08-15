# ArduBoy

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
