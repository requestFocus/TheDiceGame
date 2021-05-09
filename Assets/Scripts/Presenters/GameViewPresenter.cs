using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class GameViewPresenter : MonoBehaviour
{
    [SerializeField] private GenericButton _goBackButton;
    [SerializeField] private GenericButton _helpButton;
    [SerializeField] private GenericButton _tossDicesButton;
    [SerializeField] private RectTransform _contentTransform;
    [SerializeField] private BettingSlider _bettingSlider;
    
    private DicesManager _dicesManager;
    private GameManager _gameManager;
    private ButtonHelper _buttonHelper;
    private GenericWindow.Factory _windowFactory;
    
    private List<DicePresenter> _dicePresenters;
    private int _totalScore;

    [Inject]
    private void Construct(DicesManager dicesManager, GameManager gameManager, ButtonHelper buttonHelper, GenericWindow.Factory windowFactory)
    {
        _dicesManager = dicesManager;
        _gameManager = gameManager;
        _buttonHelper = buttonHelper;
        _windowFactory = windowFactory;
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
        // PlayerPrefs.DeleteAll();
        var window = _windowFactory.Create();
        window.transform.localScale = Vector3.one;
        window.transform.SetParent(transform);
        window.transform.localPosition = new Vector3(0, 0, 0);
    }

    private void TossDices()
    {
        DeleteOldDices();
        ClearPreviouslyOccupiedPositions();

        _dicePresenters = _dicesManager.CreateDices();
        DistributeDices();
        AddDicesValues();

        _gameManager.OnDicesTossed((int)_bettingSlider.SliderLeftValue, (int)_bettingSlider.SliderRightValue, _totalScore);

        bool isWin = _gameManager.IsWin((int) _bettingSlider.SliderLeftValue, (int) _bettingSlider.SliderRightValue,
            _totalScore);
        
        _bettingSlider.UpdateWinningDot(isWin, _totalScore);
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
    
    private void OnDestroy()
    {
        _goBackButton.onClick.RemoveAllListeners();
        _goBackButton.onLongPress.RemoveAllListeners();
        
        _helpButton.onClick.RemoveAllListeners();
        _helpButton.onLongPress.RemoveAllListeners();
        
        _tossDicesButton.onClick.RemoveAllListeners();
    }
}