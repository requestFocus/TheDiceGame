using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class GameViewPresenter : MonoBehaviour
{
    [SerializeField] private Button _goBackButton;
    [SerializeField] private Button _tossDicesButton;
    [SerializeField] private RectTransform _contentTransform;
    [SerializeField] private TextMeshProUGUI _totalScoreText;
    [SerializeField] private Slider _bettingSlider;
    [SerializeField] private TextMeshProUGUI _sliderValue;
    [SerializeField] private TextMeshProUGUI _winIndicator;

    private DicesManager _dicesManager;
    private GameManager _gameManager;

    private List<DicePresenter> _dicePresenters;
    private int _totalScore;

    [Inject]
    private void Construct(DicesManager dicesManager, GameManager gameManager)
    {
        _dicesManager = dicesManager;
        _gameManager = gameManager;
    }

    private void Start()
    {
        _goBackButton.onClick.AddListener(GoBack);
        _tossDicesButton.onClick.AddListener(TossDices);
        _bettingSlider.onValueChanged.AddListener(UpdateSlider);

        TossDices();
    }

    private void UpdateSlider(float value)
    {
        _sliderValue.text = value.ToString(CultureInfo.InvariantCulture);
    }

    private void OnDestroy()
    {
        _goBackButton.onClick.RemoveListener(GoBack);
        _tossDicesButton.onClick.RemoveListener(TossDices);
    }

    private void GoBack()
    {
        SceneManager.LoadScene("Scenes/Menu");
    }

    private void TossDices()
    {
        DeleteDices();
        ClearPreviouslyOccupiedPositions();

        _dicePresenters = _dicesManager.CreateDices();
        DistributeDices();
        AddDicesValues();

        DetermineResult();
    }

    private void AddDicesValues()
    {
        _totalScore = 0;
        foreach (var dice in _dicePresenters)
        {
            _totalScore += dice.GetScore();
        }

        _totalScoreText.text = _totalScore.ToString();
    }

    private void DistributeDices()
    {
        Rect rect = _contentTransform.rect;
        foreach (var dicePresenter in _dicePresenters)
        {
            dicePresenter.transform.SetParent(_contentTransform);
            dicePresenter.transform.localPosition
                = _dicesManager.GetUniqueRandomPosition(rect.width, rect.height, dicePresenter.GetDimensions());
            dicePresenter.transform.Rotate(0, 0, Random.Range(0, 360));
        }
    }

    private void DetermineResult()
    {
        bool isWin = _gameManager.ValidateBet((int)_bettingSlider.value, _totalScore);
        _winIndicator.text = isWin ? "WON!" : "LOST.";
    }

    private void ClearPreviouslyOccupiedPositions()
    {
        _dicesManager.ClearOccupiedPositions();
    }

    private void DeleteDices()
    {
        for (int i = 0; i < _contentTransform.childCount; i++)
        {
            Destroy(_contentTransform.GetChild(i).gameObject);
        }
    }
}