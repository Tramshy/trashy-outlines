# Trashy Outlines
Quick and easy to use outline shader for Unity, works with flat normals or smooth normals.

## Features
* Outlines for any kind of object. Works for objects with flat normals, like perfect cubes, and objects with smooth normals, like a sphere
* Silhouette that shows when object is obscured
* Easily edited outline thickness, color and alpha
* Easily animated outlines (like the outlines expanding)
* Allows for precalculating of values for the outlines

## Usage
1. Add the `OutlineSmoothNormalMapping` component to your object
2. Precalculate smooth normals using the `Recalculate Normals` button component
3. To show the outline, add a material with the `TOutline` shader and a material with the `OutlineMask` shader, or set `ShouldAddOutlineOnAwake` to true. Note: there is a base `OutlineMask` and `TOutline` material in the Resources folder
4. If you want to programmatically animate the outline thickness, use the `_Multiplier` value from 0 - 1 to decrease and increase thickness 

## License
This package is licensed under the MIT License. For more information read: `LICENSE`.

## Additional Notes
The use of [UPM Git Extension](https://github.com/mob-sakai/UpmGitExtension) is highly recommended, for easy updating of this package.
