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

    void Update()
    {
        transform.Translate(_heading.ToVector3() * _speed * Time.deltaTime);

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
            if (directionChanger != null && directionChanger.FromDirection == _heading)
            {
                float dist = Vector3.Distance(other.transform.position, transform.position);
                if (dist < Delta || dist > _prevDist)
                {
                    _heading = directionChanger.NewDirection;
                    transform.position = other.transform.position;
                    _nextDirectionChanger = null;
                }
                _prevDist = dist;
            }

            TwoWayDirectionChanger twoWayDirectionChanger = other.gameObject.GetComponent<TwoWayDirectionChanger>();
            if (twoWayDirectionChanger != null)
            {
                float dist = Vector3.Distance(other.transform.position, transform.position);
                if (dist < Delta || dist > _prevDist)
                {
                    _heading = twoWayDirectionChanger.GetOtherDirection(_heading);
                    transform.position = other.transform.position;
                    _nextDirectionChanger = null;
                }
                _prevDist = dist;
            }
        }
   }
}
