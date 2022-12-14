Water Ripple for Screens

Instructions:
- Drag the RippleEffect Script on to your Main Camera.
- Play it and click on the screen to see if its working.
- The script properties can be changed are:
	- Detect Click: true -> Detects player input (mouse left click or touch), false -> Doesn't detect player input
	- Wave Count: Maximum number of waves on screen.
	- Time Infinity: true -> Effect will be played without considering time.
	- Wave Time: Time length in seconds for each wave.
	- Wave Anim Curve: Curve that determines how the wave is going to behave through time.
	- Wave Internal Radio: Size increase of the initial radio size through time, goes from 0 to 'WaveInternalRadio' in 'WaveTime' seconds
	- Wave External Radio: Initial size of the wave.
	- Wave Scale: How much the wave affects the image.
	- Wave Speed: Velocity of the internal waves of the ripple effects, also viewed as wave amplitude.
	- Wave Frequency: Higher values shows more waves, lower values less waves.
	- Circle X Scale: Scale of the circle on the X Axis.
	- Circle Y Scale: Scale of the circle on the Y Axis.
- This package also includes a RippleGenerator Script that can spawn
ripple effect automatically. To use it drag it to the Main Camera.
- The script properties can be changed are:
	- Random Generation: true -> Ripple effects are going to be generated randomly on any position of the screens.
	- Time Between Ripple Med: Fixed time  in seconds to spawn the next ripple effect.
	- Time Between Ripple Desv: Offset time in seconds to alter the fixed time value. (Next Spawn Time is calculated as: Fixed Time + Random.Range(-1 * Offset Time, Offset Time) = Time)
	- Target position: Array of positions to spawn new ripple effects. (Position is in Screen Coordinates -> Range: (0, 0) -> (Screen Width, Screen Height))

Notes:
- If you are going to access any of the scripts from another one make sure
to include 'using WaterRippleForScreen' namespace on the other script.
- If you are going to build make sure to include 'Hidden/RippleDiffuse' to 
Always Included Shaders Array located on Edit -> Project Settings -> Graphics.

Any questions, requests, bug reports please contact: josehzz358@gmail.com

Thank you.

Jose Hernandez.