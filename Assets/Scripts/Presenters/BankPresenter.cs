using System;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Zenject;

public class BankPresenter : MonoBehaviour
{
    private Bank _model;
    private GameManager _gameManager;
    private BettingSlider _bettingSlider;

#pragma warning disable
    [SerializeField] private TextMeshProUGUI _bankBalanceText;
    [SerializeField] private TextMeshProUGUI _betAmountText;
    [SerializeField] private TextMeshProUGUI _outcomeAmountText;
    [SerializeField] private TextMeshProUGUI _gainAmountText;
    [SerializeField] private TextMeshProUGUI _mightWinAmountText;
    [SerializeField] private TextMeshProUGUI _multiplierAmountText;
    [SerializeField] private TMP_InputField _betTextField;
#pragma warning restore

    [Inject]
    private void Construct(Bank model, GameManager gameManager, BettingSlider bettingSlider)
    {
        _model = model;
        _gameManager = gameManager;
        _bettingSlider = bettingSlider;
    }

    private void Start()
    {
        RefreshBetText();
        RefreshBalanceText();
        ResetChangeableValues();

        _gameManager.DicesTossed += RefreshBalanceText; 
        _gameManager.TossingDicesCompleted += UpdatePostTossOutcome; 
        
        _gameManager.SliderUpdated += UpdatePotentialWin;
        _gameManager.SliderUpdated += UpdateMultiplier;

        _betTextField.onSubmit.AddListener(UpdateBetValue);
        _betTextField.onSubmit.AddListener(UpdatePotentialValueText);
    }

    private void RefreshBalanceText()
    {
        var balance = _model.GetBalance();
        _bankBalanceText.text = balance.ToString();
    }
    
    private void RefreshBetText()
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
    }

    private void ResetChangeableValues() 
    {
        _outcomeAmountText.text = "";
        _gainAmountText.text = "";
    }

    private void UpdatePotentialWin(int leftSliderValue, int rightSliderValue)
    {
        var possibleWinAmount = _model.CalculatePotentialOutcome(leftSliderValue, rightSliderValue);
        _mightWinAmountText.text = possibleWinAmount.ToString();
        
        ResetChangeableValues();
    }

    private void UpdatePotentialValueText(string text)
    {
        var mightWin = _model.CalculatePotentialOutcome(_bettingSlider.GetLeftSliderValue(), _bettingSlider.GetRightSliderValue());
        _mightWinAmountText.text = mightWin.ToString();
        
        ResetChangeableValues();
    }

    private void UpdateMultiplier(int leftSliderValue, int rightSliderValue)
    {
        var multiplier = _model.GetMultiplier(leftSliderValue, rightSliderValue);
        _multiplierAmountText.text = multiplier.ToString();

        ResetChangeableValues();
    }

    private void UpdateBetValue(string value)
    {
        _model.UpdateBetValue(Convert.ToInt32(value));
        RefreshBetText();
    }

    private void OnDestroy()
    {
        _gameManager.DicesTossed -= RefreshBalanceText; 
        _gameManager.TossingDicesCompleted -= UpdatePostTossOutcome; 
        
        _gameManager.SliderUpdated -= UpdatePotentialWin;
        _gameManager.SliderUpdated -= UpdateMultiplier;

        _betTextField.onSubmit.RemoveListener(UpdateBetValue);
        _betTextField.onSubmit.RemoveListener(UpdatePotentialValueText);
    }
}