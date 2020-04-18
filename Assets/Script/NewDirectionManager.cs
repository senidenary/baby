// -----------------------------------------------
// Filename: NewDirectionManager.cs
// Author:   Harold Absalom
// Licence:  GNU General Public License
// -----------------------------------------------

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NewDirectionManager : MonoBehaviour
{
    [System.Serializable]
    public struct LegalPosition
    {
        public int x;
        public int y;
        private DirectionChanger currentDirectionChanger;

        public DirectionChanger CurrentDirectionChanger
        {
            get { return currentDirectionChanger; }
            set { currentDirectionChanger = value; }
        }
    }

    public LegalPosition[] LegalPositions
    {
        get { return _legalPositions; }
    }

    #pragma warning disable 0649
    [SerializeField]
    private LegalPosition[] _legalPositions;

    [SerializeField]
    private GameObject _gridObject;

    [SerializeField]
    private GameObject _newDirectionButtonPrefab;
    #pragma warning restore 0649

    private GridLayoutGroup _gridLayoutGroup;

    private void Start()
    {
        _gridLayoutGroup = _gridObject.GetComponent<GridLayoutGroup>();

        for (int i = 0; i < 4; ++i)
        {
            AddNewDirectionButton();
        }
    }

    public void HasBeenPlaced(GameObject newDirectionButtonObject)
    {
        GameObject.Destroy(newDirectionButtonObject);
        AddNewDirectionButton();
    }

    private void AddNewDirectionButton()
    {
        GameObject newDirectionButtonObject = GameObject.Instantiate(_newDirectionButtonPrefab);
        newDirectionButtonObject.transform.SetParent(_gridLayoutGroup.transform);
        NewDirectionButton newDirectionButton = newDirectionButtonObject.GetComponent<NewDirectionButton>();
        newDirectionButton.Manager = this;
        newDirectionButton.NewHeading = RandomHeading();
        newDirectionButton.LeftTurn = RandomTurn();
    }

    private static Heading RandomHeading()
    {
        return (Heading)Random.Range(0, (float)Heading.Max);
    }

    private static bool RandomTurn()
    {
        return Random.Range(0.0f, 1.0f) > 0.5f;
    }
}
