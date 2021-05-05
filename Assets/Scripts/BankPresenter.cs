using System;
using TMPro;
using UnityEngine;
using Zenject;

public class BankPresenter : MonoBehaviour
{
    private GameConfig _gameConfig;
    private Bank _bank;
    private GameManager _gameManager;
    
    [SerializeField] private TextMeshProUGUI _ownedAmountText;
    [SerializeField] private TextMeshProUGUI _betAmountText;
    [SerializeField] private TextMeshProUGUI _outcomeAmountText;
    [SerializeField] private TextMeshProUGUI _gainAmountText;

    [Inject]
    private void Construct(GameConfig gameConfig, Bank bank, GameManager gameManager)
    {
        _gameConfig = gameConfig;
        _bank = bank;
        _gameManager = gameManager;
    }
    
    private void Start()
    {
        UpdateBankValues();

        _gameManager.DicesTossed += UpdateBankValues;
        _bank.BankUpdated += UpdateBank;
    }

    private void UpdateBank()
    {
        UpdateBankValues();
    }

    private void UpdateBankValues(int leftSliderValue = 0, int rightSliderValue = 0, int totalScore = 0)
    {
        var balance = _bank.GetBalance();
        _ownedAmountText.text = balance.ToString();
        
        var bet = (_gameConfig.BetBase * _gameConfig.AmountOfDices);
        _betAmountText.text = bet.ToString();
        
        var outcome = _bank.CalculateOutcome(leftSliderValue, rightSliderValue, totalScore);
        _outcomeAmountText.text = outcome.ToString();
        
        var gain = outcome - bet;
        _gainAmountText.text = gain.ToString();
    }

    private void OnDestroy()
    {
        _gameManager.DicesTossed -= UpdateBankValues;
    }
}
