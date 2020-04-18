// -----------------------------------------------
// Filename: NewDirectionButton.cs
// Author:   Harold Absalom
// Licence:  GNU General Public License
// -----------------------------------------------

using UnityEngine;
using UnityEngine.EventSystems;

public class NewDirectionButton : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
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
        Vector3 spawnLocation = Camera.main.ScreenToWorldPoint(eventData.position);
        spawnLocation.x = Mathf.Floor(spawnLocation.x);
        spawnLocation.y = Mathf.Floor(spawnLocation.y);
        spawnLocation.z = 0;
        GameObject obj = GameObject.Instantiate(_directionChangerPrefab, spawnLocation, Quaternion.identity) as GameObject;
        DirectionChanger directionChanger = obj.GetComponent<DirectionChanger>();

        if (directionChanger != null)
        {
            directionChanger.NewDirection = _heading;
        }

        transform.position = _startPos;
    }
}
