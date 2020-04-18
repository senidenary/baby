// -----------------------------------------------
// Filename: NewDirectionButton.cs
// Author:   Harold Absalom
// Licence:  GNU General Public License
// -----------------------------------------------

using System;
using UnityEngine;
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

    private void Start()
    {
        transform.rotation = _heading.ToQuaternion();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _startPos = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log(transform.position);
        transform.position = eventData.position;
        //Debug.Log(transform.position);
    }

    public void OnEndDrag(PointerEventData eventData)
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
        }

        transform.position = _startPos;
    }
}
