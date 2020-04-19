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

    private GameObject _nextDirectionChanger = null;
    private float _prevDist;

    private void Start()
    {
        transform.rotation = _heading.ToQuaternion();
    }

    private void Update()
    {
        transform.Translate(_heading.ToVector3() * _speed * Time.deltaTime, Space.World);

        Vector3 pos = transform.position;
        bool reachedBorder = false;
        if (pos.x < _bounds.minX)
        {
            reachedBorder = true;
            pos.x = _bounds.maxX;
        }
        else if (pos.x > _bounds.maxX)
        {
            reachedBorder = true;
            pos.x = _bounds.minX;
        }
        else if (pos.y < _bounds.minY)
        {
            reachedBorder = true;
            pos.y = _bounds.maxY;
        }
        else if (pos.y > _bounds.maxY)
        {
            reachedBorder = true;
            pos.y = _bounds.minY;
        }

        if (reachedBorder)
        {
            transform.position = pos;
            PreciousCargo preciousCargo = GetComponent<PreciousCargo>();
            if (preciousCargo != null)
            {
                preciousCargo.Die();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<DirectionChanger>() != null ||
            other.gameObject.GetComponent<TwoWayDirectionChanger>() != null)
        {
            _nextDirectionChanger = other.gameObject;
            _prevDist = Vector3.Distance(other.transform.position, transform.position);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (_nextDirectionChanger == other.gameObject)
        {
            DirectionChanger directionChanger = other.gameObject.GetComponent<DirectionChanger>();
            TwoWayDirectionChanger twoWayDirectionChanger = other.gameObject.GetComponent<TwoWayDirectionChanger>();
            bool changingDirection = false;
            Heading newHeading = Heading.Max;
            
            if (directionChanger != null && directionChanger.FromDirection == _heading)
            {
                changingDirection = true;
                newHeading = directionChanger.NewDirection;
            }
            else if (twoWayDirectionChanger != null)
            {
                changingDirection = true;
                newHeading = twoWayDirectionChanger.GetOtherDirection(_heading);
            }

            if (changingDirection)
            {
                float dist = Vector3.Distance(other.transform.position, transform.position);
                if (dist < Delta || dist > _prevDist)
                {
                    _heading = newHeading;
                    transform.position = other.transform.position;
                    dist = 0;
                    _nextDirectionChanger = null;
                }
                _prevDist = dist;

                transform.rotation = Quaternion.Lerp(_heading.ToQuaternion(), newHeading.ToQuaternion(), 1 - dist);
            }
        }
    }
}
