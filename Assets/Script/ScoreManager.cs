// -----------------------------------------------
// Filename: ScoreManager.cs
// Author:   Harold Absalom
// Licence:  GNU General Public License
// -----------------------------------------------

using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public bool CanPlaceDirections
    {
        get { return !_gameOverWindow.activeSelf; }
    }

    #pragma warning disable 0649
    [SerializeField]
    private NewDirectionManager _newDirectionManager;

    [SerializeField]
    private Text _currentScoreText;

    [SerializeField]
    private Text _highScoreText;

    [SerializeField]
    private GameObject _startInstructions;

    [SerializeField]
    private GameObject _gameOverWindow;

    [SerializeField]
    private GameObject _carNumSelectionGroup;

    [SerializeField]
    private Text _gameOverWindowReasonText;

    [SerializeField]
    private Text _gameOverWindowScoreText;

    [SerializeField]
    private MovingEntity[] _movingEntities;

    [SerializeField]
    private PreciousCargo _baby;
    #pragma warning restore 0649

    private float _currentScore = 0.0f;
    private float[] _highScores = { 0.0f, 0.0f, 0.0f, 0.0f, 0.0f };
    private int _numCars;

    // It's late, I don't want to make a struct
    private Vector3[] _originalPositions;
    private Quaternion[] _originalRotations;
    private Heading[] _originalHeadings;

    private bool _gameActive = false;

    private void Start()
    {
        _originalPositions = new Vector3[_movingEntities.Length];
        _originalRotations = new Quaternion[_movingEntities.Length];
        _originalHeadings = new Heading[_movingEntities.Length];
        for (int i = 0; i < _movingEntities.Length; ++i)
        {
            _originalPositions[i] = _movingEntities[i].transform.position;
            _originalRotations[i] = _movingEntities[i].transform.rotation;
            _originalHeadings[i] = _movingEntities[i].CurrentHeading;
        }

        ResetGame();
        SetNumCars(3);
    }

    private void Update()
    {
        if (!_gameActive)
        {
            return;
        }

        _currentScore += Time.deltaTime;
        _currentScoreText.text = _currentScore.ToString("0.00") + "s";
        if (_currentScore > _highScores[_numCars])
        {
            _highScores[_numCars] = _currentScore;
            _highScoreText.text = _currentScore.ToString("0.00") + "s";
        }
    }

    public void SetNumCars(int numCars)
    {
        _numCars = numCars - 1;
        //_movingEntities[0] should be baby
        for (int i = 1; i < _movingEntities.Length; ++i)
        {
            _movingEntities[i].gameObject.SetActive(i <= numCars);
        }
        _highScoreText.text = _highScores[_numCars].ToString("0.00") + "s";
    }

    public void GameOver(string killer)
    {
        if (!_gameActive)
        {
            return;
        }

        _gameActive = false;
        _gameOverWindow.SetActive(true);
        _gameOverWindowScoreText.text = "Score: " + _currentScore.ToString("0.00") + "s";
        foreach (MovingEntity movingEntity in _movingEntities)
        {
            movingEntity.Paused = true;
        }

        if (killer == "Car")
        {
            _gameOverWindowReasonText.text = "You let a car hit the baby!";
        }
        else if (killer == "Fire")
        {
            _gameOverWindowReasonText.text = "You set the baby on fire!";
        }
        else if (killer == "Border")
        {
            _gameOverWindowReasonText.text = "You lost the baby!";
        }
        else
        {
            Debug.LogError("Unknown killer: " + killer);
        }
    }

    public void ResetGame()
    {
        _gameOverWindow.SetActive(false);
        _startInstructions.SetActive(true);
        _carNumSelectionGroup.SetActive(true);

        _newDirectionManager.ScrambleDirections();

        foreach (MovingEntity movingEntity in _movingEntities)
        {
            movingEntity.Paused = true;
            movingEntity.HasExploded = false;
        }

        GameObject[] fires = GameObject.FindGameObjectsWithTag("Fire");
        foreach (GameObject fire in fires)
        {
            Destroy(fire);
        }

        for (int i = 0; i < _movingEntities.Length; ++i)
        {
            if (_movingEntities[i] != null)
            {
                _movingEntities[i].transform.position = _originalPositions[i];
                _movingEntities[i].transform.rotation = _originalRotations[i];
                _movingEntities[i].CurrentHeading = _originalHeadings[i];
            }
        }

        _currentScore = 0.0f;
        _currentScoreText.text = _currentScore.ToString("0.00") + "s";
    }

    public void StartGame()
    {
        if (_gameActive)
        {
            return;
        }

        _startInstructions.SetActive(false);
        _carNumSelectionGroup.SetActive(false);
        foreach (MovingEntity movingEntity in _movingEntities)
        {
            movingEntity.Paused = false;
        }
        _gameActive = true;
    }
}
