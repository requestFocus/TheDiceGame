using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Slider = UnityEngine.UI.Slider;

public class BankPresenter : MonoBehaviour
{
    private Bank _model;
    private GameManager _gameManager;
    private BettingSlider _bettingSlider;
    private GameConfig _gameConfig;

#pragma warning disable
    [SerializeField] private TextMeshProUGUI _bankBalanceText;
    [SerializeField] private TextMeshProUGUI _outcomeAmountText;
    [SerializeField] private TextMeshProUGUI _gainAmountText;
    [SerializeField] private TextMeshProUGUI _mightWinAmountText;
    [SerializeField] private TextMeshProUGUI _multiplierAmountText;
    [SerializeField] private TextMeshProUGUI _diceAmountText;
    [SerializeField] private Slider _betRegulator;
    [SerializeField] private TextMeshProUGUI _betAmountText;
    [SerializeField] private Button _betRoundingButton; 
#pragma warning restore

    [Inject]
    private void Construct(Bank model, GameManager gameManager, BettingSlider bettingSlider, GameConfig gameConfig)
    {
        _model = model;
        _gameManager = gameManager;
        _bettingSlider = bettingSlider;
        _gameConfig = gameConfig;
    }

    private void Start()
    {
        SetupPresenter();

        _gameManager.DicesTossed += RefreshBalanceText; 
        _gameManager.TossingDicesCompleted += UpdatePostTossOutcome; 
        
        _gameManager.SliderUpdated += UpdatePotentialWin;
        _gameManager.SliderUpdated += UpdateMultiplier;

        _betRegulator.onValueChanged.AddListener(value =>
        {
            UpdateBetValue((int)value);
            RefreshBetText();
            RefreshBetRegulator();
            UpdatePotentialWin(_bettingSlider.GetLeftSliderValue(), _bettingSlider.GetRightSliderValue());
        });
        
        _betRoundingButton.onClick.AddListener(OnBetRoundingClick);

        _gameManager.DiceAmountChanged += RefreshDiceAmountText;
    }

    private void SetupPresenter()
    {
        RefreshBetText();
        RefreshBalanceText();
        ResetChangeableValues();
        RefreshDiceAmountText();
        RefreshBetRegulator();
    }

    private void RefreshBalanceText()
    {
        var balance = _model.GetBalance();
        _bankBalanceText.text = balance.ToString();
    }
    
    private void RefreshBetText(float value = 0)
    {
        var bet = _model.GetCurrentBet();
        _betAmountText.text = bet.ToString();
    }

    private void UpdatePostTossOutcome(int totalScore, int leftSliderValue, int rightSliderValue)
    {
        var outcome = _model.CalculateOutcome(totalScore, leftSliderValue, rightSliderValue);
        _outcomeAmountText.text = outcome.ToString();
        var gain = outcome - _model.GetCurrentBet();
        _gainAmountText.text = gain.ToString();

        RefreshBetRegulator();
    }

    private void ResetChangeableValues() 
    {
        _outcomeAmountText.text = "";
        _gainAmountText.text = "";
    }

    private void RefreshBetRegulator()
    {
        _betRegulator.minValue = 1f;
        _betRegulator.maxValue = _model.GetBalance();
        
        _betRegulator.value = _model.GetCurrentBet();
    }

    private void UpdatePotentialWin(int leftSliderValue, int rightSliderValue)
    {
        var possibleWinAmount = _model.CalculatePotentialOutcome(leftSliderValue, rightSliderValue);
        _mightWinAmountText.text = possibleWinAmount.ToString();
        
        ResetChangeableValues();
    }

    private void OnBetRoundingClick()
    {
        int currentBet = _model.GetCurrentBet();
        int moduloBase = currentBet < 10000 ? 100 : 1000;
        int moduloRest = currentBet % moduloBase;
        int complement = moduloRest >= moduloBase / 2 ? moduloBase - moduloRest : -moduloRest;
        int newBet = currentBet + complement;
        
        UpdateBetValue(newBet);
        RefreshBetRegulator();
    }

    private void RefreshDiceAmountText()
    {
        _diceAmountText.text = _gameConfig.AmountOfDices.ToString();
    }

    private void UpdateMultiplier(int leftSliderValue, int rightSliderValue)
    {
        var multiplier = _model.GetMultiplier(leftSliderValue, rightSliderValue);
        _multiplierAmountText.text = multiplier.ToString();

        ResetChangeableValues();
    }

    private void UpdateBetValue(int value)
    {
        _model.UpdateBetValue(value);
        RefreshBetText();
    }

    private void OnDestroy()
    {
        _gameManager.DicesTossed -= RefreshBalanceText; 
        _gameManager.TossingDicesCompleted -= UpdatePostTossOutcome; 
        
        _gameManager.SliderUpdated -= UpdatePotentialWin;
        _gameManager.SliderUpdated -= UpdateMultiplier;

        _betRegulator.onValueChanged.RemoveAllListeners();
        
        _betRoundingButton.onClick.RemoveListener(OnBetRoundingClick);
        
        _gameManager.DiceAmountChanged -= RefreshDiceAmountText;
    }
}