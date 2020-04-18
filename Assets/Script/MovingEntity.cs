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
    private const float Delta = 0.01f;

    #pragma warning disable 0649
    [SerializeField]
    private float _speed;

    [SerializeField]
    private Heading _heading;
    #pragma warning restore 0649

    void Start()
    {
    }

    void Update()
    {
        transform.Translate(_heading.ToVector3() * _speed * Time.deltaTime);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        DirectionChanger directionChanger = other.gameObject.GetComponent<DirectionChanger>();
        if (directionChanger != null)
        {
            float dist = Vector3.Distance(other.transform.position, transform.position);
            if (dist < Delta)
            {
                _heading = directionChanger.NewDirection;
                transform.position = other.transform.position;
            }
        }
   }
}
