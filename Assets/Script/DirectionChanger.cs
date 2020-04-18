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

    private void UpdateDirection()
    {
        transform.rotation = _heading.ToQuaternion();
    }
}
