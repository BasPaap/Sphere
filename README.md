# Sphere
An application that provides a special effect for an interactive crystal ball in, for instance, a theatrical setting.

## Description
Using a Leap Motion device, the application detects hand movements by the user, allowing them to interact with the crystal ball. At first, a pulsating mist swirls around inside the ball. When the user puts their hands near the ball, the mists will swirl faster based on the user's hand proximity. If both hands are brought together close to the sphere, the mists part and a mystical starfield appears. Using various hand gestures, the "scryer" can summon a variety of visions that briefly appear among the stars.

## Setup
The setup for this is an opaque white ball, like a ceiling lamp fixture, onto the inside of which the images generated by the application are projected. A fish-eye lens is used on the projector to project the image onto the entire sphere, instead of just a small square in front of the projector. A leap motion device is placed near the sphere to detect hand gestures, and speakers are placed under the table to provide a stereo hum that changes with the user's hand gestures.

## Controls
In idle mode, the sphere shows a swirling mist that moves faster or slower based on the proximity of the user's hands. When both hands are brought close to the sphere, the mists part and the "vision" mode appears. Visions are represented by a Jpeg image in a special folder, ordered alphabetically on filename. Users can select the next vision by moving their right hand over their left. If the user forgets their place in the list of visions, moving the left hand over the right resets the vision to the first one in the list. The vision can be summoned by rotating both palms upwards. The vision will briefly shimmer into view before disappearing again. The sphere returns to idle mode when both hands are withdrawn.

## Leap Motion
The application does not work with the Leap Motion Orion software and upwards. Instead, the [V2 version of the Leap Motion software](https://developer.leapmotion.com/sdk/v2) is required. **NOTE:** Version 2.3.1 of the Leap Motion SDK does _not_ contain the driver installer, just the SDK source code. Download version **2.3.0** to get the executable for the driver installer. Also note that after installation, two executables need to be replaced in order for the Leap Motion to work on Windows 10. Details on this fix are available in [the forums](https://forums.leapmotion.com/t/resolved-windows-10-fall-creators-update-bugfix/6585).
