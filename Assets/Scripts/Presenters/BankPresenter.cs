using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class BankPresenter : MonoBehaviour
{
    private Bank _model;
    private GameplayManager _gameplayManager;

#pragma warning disable CS0649
    [SerializeField] private TextMeshProUGUI _bankBalanceText;
    [SerializeField] private TextMeshProUGUI _outcomeAmountText;
    [SerializeField] private TextMeshProUGUI _gainAmountText;
    [SerializeField] private TextMeshProUGUI _mightWinAmountText;
    [SerializeField] private TextMeshProUGUI _multiplierAmountText;
    [SerializeField] private TextMeshProUGUI _betAmountText;
    
    [SerializeField] private Slider _betRegulator;
    [SerializeField] private Button _betRoundingButton; 
#pragma warning restore CS0649

    [Inject]
    private void Construct(Bank model, GameplayManager gameplayManager)
    {
        _model = model;
        _gameplayManager = gameplayManager;
    }

    private void Start()
    {
        SetupPresenter();
        ResetMultiplier();
        ResetPotentialWinText();

        _gameplayManager.DicesDistributed += UpdatePostTossOutcome; 

        _gameplayManager.CellTapped += UpdatePotentialWin;
        _gameplayManager.CellTapped += UpdateMultiplier;

        _gameplayManager.DiceAmountChanged += UpdatePotentialWin;
        _gameplayManager.DiceAmountChanged += UpdateMultiplier;

        _betRegulator.onValueChanged.AddListener(value =>
        {
            UpdateBetValue((int)value);
            RefreshBetText();
            RefreshBetRegulator();
            UpdatePotentialWin(_gameplayManager.GetSelectedCellsPresenters());
        });
        
        _betRoundingButton.onClick.AddListener(OnBetRoundingClick);
    }

    private void SetupPresenter()
    {
        RefreshBetText();
        RefreshBalanceText();
        ResetChangeableValuesTexts();
        RefreshBetRegulator();
    }

    public void RefreshBalanceText()
    {
        var balance = _model.GetBalance();
        _bankBalanceText.text = balance.ToString();
    }
    
    private void RefreshBetText()
    {
        var bet = _model.GetCurrentBet();
        _betAmountText.text = bet.ToString();
    }

    private void UpdatePostTossOutcome(int totalScore, List<BettingCellPresenter> selectedCellPresenter)
    {
        var outcome = _model.CalculateOutcome(totalScore, selectedCellPresenter);
        _outcomeAmountText.text = outcome.ToString();
        var gain = outcome - _model.GetCurrentBet();
        _gainAmountText.text = gain.ToString();

        RefreshBetRegulator();
    }

    private void ResetChangeableValuesTexts()
    {
        _outcomeAmountText.text = "";
        _gainAmountText.text = "";
    }
    
    private void ResetPotentialWinText()
    {
        _mightWinAmountText.text = "0";     // poprawić
    }

    private void ResetMultiplier()
    {
        _multiplierAmountText.text = "";
    }

    private void RefreshBetRegulator()
    {
        _betRegulator.minValue = 1f;
        _betRegulator.maxValue = _model.GetBalance();
        
        _betRegulator.value = _model.GetCurrentBet();
    }

    private void UpdatePotentialWin(List<BettingCellPresenter> selectedCellPresenters)
    {
        if (selectedCellPresenters == null)
        {
            _mightWinAmountText.text = 0.ToString();
        }
        
        if (selectedCellPresenters != null)
        {
            int possibleWinAmount = _model.CalculatePotentialOutcome(selectedCellPresenters);
            _mightWinAmountText.text = possibleWinAmount.ToString();
        }

        ResetChangeableValuesTexts();
    }

    private void OnBetRoundingClick()
    {
        int currentBet = _model.GetCurrentBet();
        int roundingBase = currentBet < 10000 ? 100 : 1000;
        int differenceToRound = currentBet % roundingBase;
        int complement = differenceToRound >= roundingBase / 2 ? roundingBase - differenceToRound : -differenceToRound;
        int newBet = currentBet + complement;

        if (newBet == 0)
        {
            newBet = 1;
        }
        
        UpdateBetValue(newBet);
        RefreshBetRegulator();
    }

    private void UpdateMultiplier(List<BettingCellPresenter> selectedCellPresenters)
    {
        if (selectedCellPresenters == null)
        {
            _multiplierAmountText.text = 0.ToString();
        }

        if (selectedCellPresenters != null)
        {
            int multiplier = _model.GetMultiplier(selectedCellPresenters);
            _multiplierAmountText.text = multiplier.ToString();
        }

        ResetChangeableValuesTexts();
    }

    private void UpdateBetValue(int value)
    {
        _model.UpdateBetValue(value);
        RefreshBetText();
    }

    private void OnDestroy()
    {
        _gameplayManager.DicesDistributed -= UpdatePostTossOutcome; 
        
        _gameplayManager.CellTapped -= UpdatePotentialWin;
        _gameplayManager.CellTapped -= UpdateMultiplier;

        _gameplayManager.DiceAmountChanged -= UpdatePotentialWin;
        _gameplayManager.DiceAmountChanged -= UpdateMultiplier;

        _betRegulator.onValueChanged.RemoveAllListeners();
        
        _betRoundingButton.onClick.RemoveListener(OnBetRoundingClick);
    }

    private void UpdatePotentialWin()
    {
        UpdatePotentialWin(_gameplayManager.GetSelectedCellsPresenters());
    }

    private void UpdateMultiplier()
    {
        UpdateMultiplier(_gameplayManager.GetSelectedCellsPresenters());
    }
}