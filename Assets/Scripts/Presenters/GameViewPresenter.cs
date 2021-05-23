using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class GameViewPresenter : MonoBehaviour
{
#pragma warning disable CS0649
    [SerializeField] private GenericButton _goBackButton;
    [SerializeField] private GenericButton _helpButton;
    [SerializeField] private GenericButton _tossDicesButton;
    [SerializeField] private RectTransform _contentTransform;
    [SerializeField] private BettingSliderPresenter _bettingSliderPresenter;
#pragma warning restore CS0649

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

    private async void TossDices()
    {
        DeleteOldDices();
        ClearPreviouslyOccupiedPositions();

        _dicePresenters = _dicesManager.CreateDices();
        await DistributeDices();

        AddDicesValues();

        _gameManager.OnDicesToss(_totalScore, _bettingSlider.GetLeftSliderValue(),
            _bettingSlider.GetRightSliderValue());

        bool isWin = _gameManager.IsWin(_totalScore, _bettingSlider.GetLeftSliderValue(),
            _bettingSlider.GetRightSliderValue());
        bool isWithinWinningRange =
            _gameManager.IsWithinWiningRange(_bettingSlider.GetLeftSliderValue(), _bettingSlider.GetRightSliderValue());
        _bettingSliderPresenter.UpdateWinningDot(isWin & isWithinWinningRange, _totalScore);

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
            _totalScore += dice.GetDiceValue();
        }
    }

    // TODO: MOVE TO DICES MANAGER
    private async UniTask DistributeDices()
    {
        Rect contentRect = _contentTransform.rect;
        Sequence sequence = DOTween.Sequence();
        
        foreach (DicePresenter dicePresenter in _dicePresenters)
        {
            var targetDotsAmount = dicePresenter.GetGeneratedRandomId();
            Transform diceTransform = dicePresenter.transform;

            Vector2 commonStartingPosition = new Vector3(contentRect.width, -contentRect.height);
            Vector2 uniqueRandomLandingPosition = _dicesManager.GetUniqueRandomPosition(contentRect.width,
                contentRect.height, dicePresenter.GetDimensions());
            
            diceTransform.SetParent(_contentTransform, true);
            diceTransform.localScale = Vector3.one;
            diceTransform.localPosition = commonStartingPosition;

            float angle = Mathf.Atan2(commonStartingPosition.y - uniqueRandomLandingPosition.y, commonStartingPosition.x - uniqueRandomLandingPosition.x) * Mathf.Rad2Deg;
            dicePresenter.GetDiceImage().transform.rotation = Quaternion.Euler (new Vector3(0f,0f,angle));
            dicePresenter.GetAnimatedSidesContainerTransform().transform.rotation = Quaternion.Euler (new Vector3(0f,0f,angle));

            sequence = DOTween.Sequence()
                .Append(diceTransform.DOLocalMove(uniqueRandomLandingPosition, 1.2f)) // SET EASE
                .InsertCallback(0f, () =>
                {
                    dicePresenter.AnimateDiceMovement(targetDotsAmount);
                });
        }

        await sequence.Play().AsyncWaitForCompletion();
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