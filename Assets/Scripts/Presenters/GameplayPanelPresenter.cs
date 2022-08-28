using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Vector3 = UnityEngine.Vector3;

public class GameplayPanelPresenter : MonoBehaviour
{
#pragma warning disable CS0649
    [SerializeField] private GenericButton _goBackButton;
    [SerializeField] private GenericButton _helpButton;
    [SerializeField] private GenericButton _tossDicesButton;
    [SerializeField] private Image _tossDicesImage;
    [SerializeField] private RectTransform _contentTransform;
    
    [SerializeField] private WinScoreIndicatorPresenter _winScoreIndicatorPresenter;
    [SerializeField] private BankPresenter _bankPresenter;
#pragma warning restore CS0649

    private ButtonHelper _buttonHelper;
    private UiManager _uiManager;
    private BettingSystem _bettingSystem;

    private GameplayPanel _model;
    
    [Inject]
    private void Construct(GameplayPanel model, 
        ButtonHelper buttonHelper,
        UiManager uiManager,
        BettingSystem bettingSystem)
    {
        _model = model;
        
        _uiManager = uiManager;
        _bettingSystem = bettingSystem;
        
        _buttonHelper = buttonHelper;
    }

    private void Start()
    {
        _goBackButton.onClick.AddListener(() => _buttonHelper.OnButtonClick(_goBackButton, _model.GoBack));
        _goBackButton.onLongPress.AddListener(() => _buttonHelper.OnButtonLongPress(_goBackButton, () => { }));

        _helpButton.onClick.AddListener(() => _buttonHelper.OnButtonClick(_helpButton, _model.ShowHelp));
        _helpButton.onLongPress.AddListener(() => _buttonHelper.OnButtonLongPress(_helpButton, () => { }));

        _tossDicesButton.onClick.AddListener(() => _buttonHelper.OnButtonClick(_tossDicesButton, Toss, true));

        ValidateTossButtonState(_model.GetSelectedCellsPresenters());
        _bettingSystem.CellTapped += ValidateTossButtonState;
        _model.DiceAmountChangedRedirect += () => ValidateTossButtonState(_model.GetSelectedCellsPresenters());
    }

    private async void Toss()
    {
        OnTurnStart();
        
        DeleteOldDices();
        _model.SetupDices();
        await _model.DistributeDices(_contentTransform);
        _model.OnDicesDistributed();

        OnTurnEnd();

        CheckBankForBankruptcy();
    }

    private void OnTurnStart()
    {
        _winScoreIndicatorPresenter.HideWinScoreIndicator();
        _bettingSystem.UnmarkWinningCell();
    }

    private void OnTurnEnd()
    {
        _bankPresenter.RefreshBalanceText();
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
    
    private void ValidateTossButtonState(List<BettingCellPresenter> selectedCellsPresenters)
    {
        Color faded = new Color(_tossDicesImage.color.r, _tossDicesImage.color.g, _tossDicesImage.color.b, 0.3f);
        Color full = new Color(_tossDicesImage.color.r, _tossDicesImage.color.g, _tossDicesImage.color.b, 1f);
        
        if (selectedCellsPresenters == null || selectedCellsPresenters.Count == 0)
        {
            _tossDicesImage.color = faded;
            _tossDicesButton.interactable = false;
        }
        else
        {
            _tossDicesImage.color = full;
            _tossDicesButton.interactable = true;
        }
    }
    
    private void OnDestroy()
    {
        _goBackButton.onClick.RemoveAllListeners();
        _goBackButton.onLongPress.RemoveAllListeners();
        
        _helpButton.onClick.RemoveAllListeners();
        _helpButton.onLongPress.RemoveAllListeners();
        
        _tossDicesButton.onClick.RemoveAllListeners();

        _bettingSystem.CellTapped -= ValidateTossButtonState;
        void UnsubscribeFromValidateTossButton() => ValidateTossButtonState(_model.GetSelectedCellsPresenters());
        _model.DiceAmountChangedRedirect -= UnsubscribeFromValidateTossButton;
    }

    private void CheckBankForBankruptcy()
    {
        if (!_model.CanContinue())
        {
            _uiManager.ShowWindow<GameOverWindow>();
        }
    }
}