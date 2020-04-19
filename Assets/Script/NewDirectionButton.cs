// -----------------------------------------------
// Filename: NewDirectionButton.cs
// Author:   Harold Absalom
// Licence:  GNU General Public License
// -----------------------------------------------

using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NewDirectionButton : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    #pragma warning disable 0649
    [SerializeField]
    private GameObject _directionChangerPrefab;

    [SerializeField]
    private Heading _heading;
    #pragma warning restore 0649

    public Heading NewHeading
    {
        set { _heading = value; }
    }

    public NewDirectionManager Manager
    {
        set { _newDirectionManager = value; }
    }

    private NewDirectionManager _newDirectionManager;
    private Vector3 _startPos;
    private bool _isBeingDragged = false;

    private void Start()
    {
        transform.rotation = _heading.ToQuaternion();
    }

    private void Update()
    {
        if (_isBeingDragged && !_newDirectionManager.CanPlaceNewDirections())
        {
            transform.position = _startPos;
            _isBeingDragged = false;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_newDirectionManager.CanPlaceNewDirections())
        {
            _startPos = transform.position;
            _isBeingDragged = true;
        }
        else
        {
            eventData.pointerDrag = null;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_newDirectionManager.CanPlaceNewDirections())
        {
            transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_newDirectionManager.CanPlaceNewDirections())
        {
            Vector3 spawnLocation = Camera.main.ScreenToWorldPoint(eventData.position);
            NewDirectionManager.LegalPosition positionToTest;
            positionToTest.x = Mathf.FloorToInt(spawnLocation.x + 0.5f);
            positionToTest.y = Mathf.FloorToInt(spawnLocation.y + 0.5f);

            int positionIndex = Array.FindIndex(_newDirectionManager.LegalPositions, p => p.x == positionToTest.x && p.y == positionToTest.y);
            if (positionIndex != -1)
            {
                DirectionChanger directionChanger = null;

                if (_newDirectionManager.LegalPositions[positionIndex].CurrentDirectionChanger != null)
                {
                    directionChanger = _newDirectionManager.LegalPositions[positionIndex].CurrentDirectionChanger;
                }
                else
                {
                    spawnLocation.x = positionToTest.x;
                    spawnLocation.y = positionToTest.y;
                    spawnLocation.z = 0;

                    GameObject obj = GameObject.Instantiate(_directionChangerPrefab, spawnLocation, Quaternion.identity);
                    directionChanger = obj.GetComponent<DirectionChanger>();

                    _newDirectionManager.LegalPositions[positionIndex].CurrentDirectionChanger = directionChanger;
                }

                if (directionChanger != null)
                {
                    directionChanger.NewDirection = _heading;
                }

                _newDirectionManager.HasBeenPlaced(gameObject);
                _isBeingDragged = false;
            }
            else
            {
                transform.position = _startPos;
            }
        }
    }
}
