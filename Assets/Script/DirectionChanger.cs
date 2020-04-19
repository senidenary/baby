// -----------------------------------------------
// Filename: DirectionChanger.cs
// Author:   Harold Absalom
// Licence:  GNU General Public License
// -----------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionChanger : MonoBehaviour
{
    public Heading NewDirection
    {
        get { return _heading; }
        set
        {
            _heading = value;
            UpdateDirection();
        }
    }

    [SerializeField]
    private Heading _heading;

    private void Start()
    {
        UpdateDirection();
    }

    public bool IsFromDirection(Heading fromDirection)
    {
        switch (_heading)
        {
            case Heading.Up:
                return (fromDirection == Heading.Left || fromDirection == Heading.Right);
            case Heading.Down:
                return (fromDirection == Heading.Left || fromDirection == Heading.Right);
            case Heading.Left:
                return (fromDirection == Heading.Up || fromDirection == Heading.Down);
            case Heading.Right:
                return (fromDirection == Heading.Up || fromDirection == Heading.Down);
        }
        
        return false;
    }

    private void UpdateDirection()
    {
        transform.rotation = _heading.ToQuaternion();
    }
}
