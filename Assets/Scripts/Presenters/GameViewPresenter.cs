using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class GameViewPresenter : MonoBehaviour
{
#pragma warning disable
    [SerializeField] private GenericButton _goBackButton;
    [SerializeField] private GenericButton _helpButton;
    [SerializeField] private GenericButton _tossDicesButton;
    [SerializeField] private RectTransform _contentTransform;
    [SerializeField] private BettingSliderPresenter _bettingSliderPresenter;
#pragma warning restore
    
    private DicesManager _dicesManager;
    private GameManager _gameManager;
    private ButtonHelper _buttonHelper;
    private UiManager _uiManager;
    private BettingSlider _bettingSlider;
    
    private List<DicePresenter> _dicePresenters;
    private int _totalScore;

    [Inject]
    private void Construct(DicesManager dicesManager, GameManager gameManager, ButtonHelper buttonHelper,
        UiManager uiManager, BettingSlider bettingSlider)
    {
        _dicesManager = dicesManager;
        _gameManager = gameManager;
        _buttonHelper = buttonHelper;
        _uiManager = uiManager;
        _bettingSlider = bettingSlider;
    }

    private void Start()
    {
        _goBackButton.onClick.AddListener(() => _buttonHelper.OnButtonClick(_goBackButton, GoBack));
        _goBackButton.onLongPress.AddListener(() => _buttonHelper.OnButtonLongPress(_goBackButton, () => { }));

        _helpButton.onClick.AddListener(() => _buttonHelper.OnButtonClick(_helpButton, ShowHelp));
        _helpButton.onLongPress.AddListener(() => _buttonHelper.OnButtonLongPress(_helpButton, () => { }));
        
        _tossDicesButton.onClick.AddListener(() => _buttonHelper.OnButtonClick(_tossDicesButton, TossDices));
    }

    private void GoBack()
    {
        SceneManager.LoadScene("Scenes/MenuScene");
    }
    
    private void ShowHelp()
    {
        var howToPlayWindow = _uiManager.ShowWindow<HowToPlayWindow>();
    }

    private void TossDices()
    {
        DeleteOldDices();
        ClearPreviouslyOccupiedPositions();

        _dicePresenters = _dicesManager.CreateDices();
        DistributeDices();
        
        AddDicesValues();

        _gameManager.OnDicesToss(_totalScore, _bettingSlider.GetLeftSliderValue(), _bettingSlider.GetRightSliderValue());

        bool isWin = _gameManager.IsWin(_totalScore, _bettingSlider.GetLeftSliderValue(), _bettingSlider.GetRightSliderValue());
        _bettingSliderPresenter.UpdateWinningDot(isWin, _totalScore);

        _gameManager.OnTurnEnd();
        
        if (!CanContinue())
        {
            var gameOverWindow = _uiManager.ShowWindow<GameOverWindow>();
        }
    }

    private void AddDicesValues()
    {
        _totalScore = 0;
        foreach (var dice in _dicePresenters)
        {
            _totalScore += dice.GetScore();
        }
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
            dicePresenter.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.InBounce).From(Vector3.zero);
        }
    }

    private void ClearPreviouslyOccupiedPositions()
    {
        _dicesManager.ClearOccupiedPositions();
    }

    private void DeleteOldDices()
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

    private bool CanContinue()
    {
        return PlayerPrefs.GetInt("Balance") > 0;
    }
    
    private void OnDestroy()
    {
        _goBackButton.onClick.RemoveAllListeners();
        _goBackButton.onLongPress.RemoveAllListeners();
        
        _helpButton.onClick.RemoveAllListeners();
        _helpButton.onLongPress.RemoveAllListeners();
        
        _tossDicesButton.onClick.RemoveAllListeners();
    }
}