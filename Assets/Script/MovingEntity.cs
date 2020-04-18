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

    [System.Serializable]
    public struct GridBounds
    {
        public float minX;
        public float minY;
        public float maxX;
        public float maxY;
    }

    #pragma warning disable 0649
    [SerializeField]
    private float _speed;

    [SerializeField]
    private Heading _heading;

    [SerializeField]
    private GridBounds _bounds;
    #pragma warning restore 0649

    void Update()
    {
        transform.Translate(_heading.ToVector3() * _speed * Time.deltaTime);

        Vector3 pos = transform.position;
        if (pos.x < _bounds.minX)
        {
            pos.x = _bounds.maxX;
        }
        else if (pos.x > _bounds.maxX)
        {
            pos.x = _bounds.minX;
        }
        else if (pos.y < _bounds.minY)
        {
            pos.y = _bounds.maxY;
        }
        else if (pos.y > _bounds.maxY)
        {
            pos.y = _bounds.minY;
        }
        transform.position = pos;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        DirectionChanger directionChanger = other.gameObject.GetComponent<DirectionChanger>();
        if (directionChanger != null && directionChanger.FromDirection == _heading)
        {
            float dist = Vector3.Distance(other.transform.position, transform.position);
            if (dist < Delta)
            {
                _heading = directionChanger.NewDirection;
                transform.position = other.transform.position;
            }
        }

        TwoWayDirectionChanger twoWayDirectionChanger = other.gameObject.GetComponent<TwoWayDirectionChanger>();
        if (twoWayDirectionChanger != null)
        {
            float dist = Vector3.Distance(other.transform.position, transform.position);
            if (dist < Delta)
            {
                _heading = twoWayDirectionChanger.GetOtherDirection(_heading);
                transform.position = other.transform.position;
            }
        }
   }
}
