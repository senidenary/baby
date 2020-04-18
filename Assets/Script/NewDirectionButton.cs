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
    [SerializeField]
    private GameObject _newDirectionManager;

    [SerializeField]
    private GameObject _directionChangerPrefab;

    [SerializeField]
    private Heading _heading;

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
        NewDirectionManager newDirectionManager = _newDirectionManager.GetComponent<NewDirectionManager>();

        Vector3 spawnLocation = Camera.main.ScreenToWorldPoint(eventData.position);
        NewDirectionManager.LegalPosition positionToTest;
        positionToTest.x = Mathf.FloorToInt(spawnLocation.x + 0.5f);
        positionToTest.y = Mathf.FloorToInt(spawnLocation.y + 0.5f);

        int positionIndex = Array.FindIndex(newDirectionManager._legalPositions, p => p.x == positionToTest.x && p.y == positionToTest.y);
        if (positionIndex != -1)
        {
            DirectionChanger directionChanger = null;

            if (newDirectionManager._legalPositions[positionIndex].currentDirectionChanger != null)
            {
                directionChanger = newDirectionManager._legalPositions[positionIndex].currentDirectionChanger;
            }
            else
            {
                spawnLocation.x = positionToTest.x;
                spawnLocation.y = positionToTest.y;
                spawnLocation.z = 0;

                GameObject obj = GameObject.Instantiate(_directionChangerPrefab, spawnLocation, Quaternion.identity) as GameObject;
                directionChanger = obj.GetComponent<DirectionChanger>();

                newDirectionManager._legalPositions[positionIndex].currentDirectionChanger = directionChanger;
            }

            if (directionChanger != null)
            {
                directionChanger.NewDirection = _heading;
            }
        }

        transform.position = _startPos;
    }
}
