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

    [System.Serializable]
    public struct RespawnPosition
    {
        public int x;
        public int y;
        public Heading heading;
    }

    public bool Paused
    {
        set { _paused = value; }
    }

    public bool HasExploded
    {
        set { _hasExploded = value; }
    }

    public Heading CurrentHeading
    {
        get { return _heading; }
        set { _heading = value; }
    }

    #pragma warning disable 0649
    [SerializeField]
    private float _speed;

    [SerializeField]
    private Heading _heading;

    [SerializeField]
    private RespawnPosition[] _respawnPositions;

    [SerializeField]
    private GridBounds _bounds;

    [SerializeField]
    private GameObject _explosionPrefab;
    #pragma warning restore 0649

    private GameObject _nextDirectionChanger = null;
    private float _prevDist;

    private bool _paused = true;

    private bool _hasExploded = false;

    private void Start()
    {
        transform.rotation = _heading.ToQuaternion();
    }

    private void Update()
    {
        if (_paused)
        {
            return;
        }

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
                preciousCargo.Die("Border");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_paused)
        {
            return;
        }

        if (other.gameObject.GetComponent<DirectionChanger>() != null ||
            other.gameObject.GetComponent<TwoWayDirectionChanger>() != null)
        {
            _nextDirectionChanger = other.gameObject;
            _prevDist = Vector3.Distance(other.transform.position, transform.position);
        }
        else
        {
            if (tag == "Car" && (other.tag == "Car" || other.tag == "Fire"))
            {
                MovingEntity otherEntity = other.gameObject.GetComponent<MovingEntity>();
                if (otherEntity)
                {
                    otherEntity.Explode();
                }
                Explode();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (_paused)
        {
            return;
        }

        if (_nextDirectionChanger == other.gameObject)
        {
            DirectionChanger directionChanger = other.gameObject.GetComponent<DirectionChanger>();
            TwoWayDirectionChanger twoWayDirectionChanger = other.gameObject.GetComponent<TwoWayDirectionChanger>();
            bool changingDirection = false;
            Heading newHeading = Heading.Max;
            
            if (directionChanger != null && directionChanger.IsFromDirection(_heading))
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

    public void Explode()
    {
        if (!_hasExploded && _explosionPrefab)
        {
            Vector3 explosionPosition = transform.position;
            explosionPosition.z -= 2;

            int respawnPositionIndex = GetAvailableRespawnPositionIndex();
            transform.position = new Vector3(_respawnPositions[respawnPositionIndex].x, _respawnPositions[respawnPositionIndex].y, 0);
            transform.rotation =  _respawnPositions[respawnPositionIndex].heading.ToQuaternion();
            _heading = _respawnPositions[respawnPositionIndex].heading;
            //_hasExploded = true;

            GameObject explosionObject = GameObject.Instantiate(_explosionPrefab, explosionPosition, Quaternion.identity);
        }
    }

    // I'm sure this is something we were told never to do in CS class
    private int GetAvailableRespawnPositionIndex()
    {
        const float minDist = 1.0f;
        GameObject[] cars = GameObject.FindGameObjectsWithTag("Car");
        List<int> validIndices = new List<int>();

        for (int i = 0; i < _respawnPositions.Length; ++i)
        {
            bool valid = true;
            foreach (GameObject car in cars)
            {
                if (Mathf.Abs(car.transform.position.x - _respawnPositions[i].x) < minDist &&
                    Mathf.Abs(car.transform.position.y - _respawnPositions[i].y) < minDist)
                {
                    valid = false;
                    break;
                }
            }

            if (valid)
            {
                validIndices.Add(i);
            }
        }

        int validIndexIndex = Random.Range(0, validIndices.Count);

        return validIndices[validIndexIndex];
    }
}
