// -----------------------------------------------
// Filename: MovingEntity.cs
// Author:   Harold Absalom
// Licence:  GNU General Public License
// -----------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingEntity : MonoBehaviour
{
    [SerializeField]
    private float _speed;

    [SerializeField]
    private Heading _heading;

    void Start()
    {
    }

    void Update()
    {
        transform.Translate(_heading.ToVector3() * _speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DirectionChanger directionChanger = other.gameObject.GetComponent<DirectionChanger>();
        if (directionChanger != null)
        {
            _heading = directionChanger.NewDirection;
        }
   }
}
