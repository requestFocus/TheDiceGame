using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class GameViewPresenter : MonoBehaviour
{
    [SerializeField] private Button _goBackButton;
    [SerializeField] private Button _helpButton;
    [SerializeField] private Button _tossDicesButton;
    
    [SerializeField] private RectTransform _contentTransform;
    
    [SerializeField] private TextMeshProUGUI _totalScoreText;
    [SerializeField] private TextMeshProUGUI _winIndicator2;
    
    [SerializeField] private BettingSlider _bettingSlider;
    
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
        _helpButton.onClick.AddListener(ShowHelp);
        _tossDicesButton.onClick.AddListener(TossDices);
    }

    private void GoBack()
    {
        SceneManager.LoadScene("Scenes/Menu");
    }
    
    private void ShowHelp()
    {
    }

    private void TossDices()
    {
        DeleteDices();
        ClearPreviouslyOccupiedPositions();

        _dicePresenters = _dicesManager.CreateDices();
        DistributeDices();
        AddDicesValues();

        DetermineResult();
        _bettingSlider.UpdateWinningDot(_totalScore);
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
        foreach (DicePresenter dicePresenter in _dicePresenters)
        {
            dicePresenter.transform.SetParent(_contentTransform);
            dicePresenter.transform.localPosition
                = _dicesManager.GetUniqueRandomPosition(rect.width, rect.height, dicePresenter.GetDimensions());
            dicePresenter.GetDiceImage().transform.Rotate(0, 0, Random.Range(0, 360)); //         MOVE TO DICES MANAGER?
            dicePresenter.GetDiceImage().transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.InBounce).From(Vector3.zero);
        }
    }

    private void DetermineResult()
    {
        bool isWin = _gameManager.ValidateBet((int)_bettingSlider.SliderLeftValue, (int)_bettingSlider.SliderRightValue, _totalScore);
        _winIndicator2.text = isWin ? "WON!" : "LOST.";
    }

    private void ClearPreviouslyOccupiedPositions()
    {
        _dicesManager.ClearOccupiedPositions();
    }

    private void DeleteDices()
    {
        for (int i = 0; i < _contentTransform.childCount; i++)
        {
            Transform diceTransform = _contentTransform.GetChild(i);
            diceTransform.DOScale(Vector3.zero, 0.1f)
                .OnComplete(() =>
            {
                Destroy(diceTransform.gameObject);
            });
        }
    }
    
    private void OnDestroy()
    {
        _goBackButton.onClick.RemoveListener(GoBack);
        _tossDicesButton.onClick.RemoveListener(TossDices);
    }
}