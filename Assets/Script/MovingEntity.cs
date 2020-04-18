// -----------------------------------------------
// Filename: MovingEntity.cs
// Author:   Harold Absalom
// Licence:  GNU General Public License
// -----------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Direction))]
public class MovingEntity : MonoBehaviour
{
    [SerializeField]
    private float _speed;

    private Direction _direction;

    void Start()
    {
        _direction = GetComponent<Direction>();
    }

    void Update()
    {
        transform.Translate(_direction.ToVector3() * _speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DirectionChanger directionChanger = other.gameObject.GetComponent<DirectionChanger>();
        if (directionChanger != null)
        {
            _direction = directionChanger.NewDirection;
        }
   }
}
