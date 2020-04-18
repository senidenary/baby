// -----------------------------------------------
// Filename: Heading.cs
// Author:   Harold Absalom
// Licence:  GNU General Public License
// -----------------------------------------------

using UnityEngine;

public enum Heading
{
    Up,
    Down,
    Left,
    Right,
    Max
}

public static class HeadingExtensionMethods
{
    public static Vector3 ToVector3(this Heading heading)
    {
        switch (heading)
        {
            case Heading.Up:
                return new Vector3( 0,  1,  0);
            case Heading.Down:
                return new Vector3( 0, -1,  0);
            case Heading.Left:
                return new Vector3(-1,  0,  0);
            case Heading.Right:
                return new Vector3( 1,  0,  0);
        }

        Debug.LogError("Invalid Heading");
        return new Vector3(0, 0, 0);
    }

    public static Quaternion ToQuaternion(this Heading heading)
    {
        switch (heading)
        {
            case Heading.Up:
                return Quaternion.Euler(0, 0,   0);
            case Heading.Down:
                return Quaternion.Euler(0, 0, 180);
            case Heading.Left:
                return Quaternion.Euler(0, 0,  90);
            case Heading.Right:
                return Quaternion.Euler(0, 0, 270);
        }

        Debug.LogError("Invalid Heading");
        return Quaternion.Euler(0, 0, 0);
    }
}
