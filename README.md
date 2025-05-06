# Trashy Outlines
Quick and easy to use outline shader for Unity, works with flat normals or smooth normals.

## Features
* Outlines for any kind of object. Works for objects with flat normals, like perfect cubes, and objects with smooth normals, like a sphere
* Silhouette that shows when object is obscured
* Easily edited outline thickness, color and alpha
* Easily animated outlines (like the outlines expanding)
* Allows for precalculating of values for the outlines

## Installation
This repository is installed as a package for Unity.
1. `Open Window` > `Package Manager`.
2. Click `+`.
3. Select Add Package from git URL.
4. Paste `https://github.com/Tramshy/trashy-outlines.git`.
5. Click Add.

NOTE: To do this you need Git installed on your computer.

## Usage
1. Add the `OutlineSmoothNormalMapping` component to your object
2. Precalculate smooth normals using the `Recalculate Normals` button component
3. To show the outline, add a material with the `TOutline` shader and a material with the `OutlineMask` shader, or set `ShouldAddOutlineOnAwake` to true. Note: there is a base `OutlineMask` and `TOutline` material in the Resources folder
4. If you want to programmatically animate the outline thickness, use the `_Multiplier` value from 0 - 1 to decrease and increase thickness 

### Material settings
The outline material and mask material have their own Enums that determine how the outline will be rendered. These are set per material, so if you want different kinds of outlines you will need multiple materials.
You can also set the default outlines for an object to use if you use the `ShouldAddOutlineOnAwake` bool on the component, if you leave the fields as null it will use the default outline options.

## License
This package is licensed under the MIT License. For more information read: `LICENSE`.

## Additional Notes
There are warning messages and such for the `OutlineSmoothNormalMapping` component, but they randomly broke so just ignore them please!
