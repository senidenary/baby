// -----------------------------------------------
// Filename: Direction.cs
// Author:   Harold Absalom
// Licence:  GNU General Public License
// -----------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Direction : MonoBehaviour
{
    public enum Heading
    {
        Up,
        Down,
        Left,
        Right
    }

    public Heading CurrentHeading
    {
        get { return _heading; }
        set { _heading = value; }
    }

    [SerializeField]
    private Heading _heading;

    public Vector3 ToVector3()
    {
        switch (_heading)
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

    public Quaternion ToQuaternion()
    {
        switch (_heading)
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
