// -----------------------------------------------
// Filename: DirectionChanger.cs
// Author:   Harold Absalom
// Licence:  GNU General Public License
// -----------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Direction))]
public class DirectionChanger : MonoBehaviour
{
    public Direction NewDirection
    {
        get { return _direction; }
    }

    private Direction _direction;

    void Start()
    {
         _direction = GetComponent<Direction>();
        transform.rotation = _direction.ToQuaternion();
    }
}
