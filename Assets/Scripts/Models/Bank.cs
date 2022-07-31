using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class Bank : IInitializable, IDisposable
{
    private GameConfig _gameConfig;
    private GameplayManager _gameplayManager;
    
    private const string BALANCE = "Balance";

    private Bank(GameConfig gameConfig, GameplayManager gameplayManager)
    {
        _gameConfig = gameConfig;
        _gameplayManager = gameplayManager;
    }
    
    public void Initialize()
    {
        _gameplayManager.DicesDistributed += DeductBet;
        _gameplayManager.DicesDistributed += UpdateBank;
    }
    
    private void UpdateBank(int dicesSum, List<BettingCellPresenter> selectedCellsPresenters)
    {
        var currentBalance = PlayerPrefs.GetInt(BALANCE, 200);
        var newBalance = currentBalance + CalculateOutcome(dicesSum, selectedCellsPresenters);
        PlayerPrefs.SetInt(BALANCE, newBalance);
    }

    public int CalculateOutcome(int dicesSum, List<BettingCellPresenter> selectedCellPresenters)
    {
        if (!selectedCellPresenters.Select(cell => cell.GetCellId()).Contains(dicesSum - 1))
        {
            return 0;
        }

        return CalculatePotentialOutcome(selectedCellPresenters);
    }

    public int CalculatePotentialOutcome(List<BettingCellPresenter> selectedCellPresenters)
    {
        return GetMultiplier(selectedCellPresenters) * _gameConfig.BetBase * _gameConfig.AmountOfDices;
    }
    
    public int GetMultiplier(List<BettingCellPresenter> selectedCellPresenters)
    {
        int range = selectedCellPresenters.Count;

        switch (range)
        {
            case 0:
                return _gameConfig.WinMultiplierForNoSelection;
            case 1:
                return _gameConfig.WinMultiplierForOneSelected;
            case 2:
                return _gameConfig.WinMultiplierForTwoSelected;
            case 3:
                return _gameConfig.WinMultiplierForThreeSelected;
            default:
                return 0;
        } 
    }

    public int GetBalance()
    {
        return PlayerPrefs.GetInt(BALANCE);
    }

    public int GetCurrentBet()
    {
        return _gameConfig.BetBase;
    }

    public void UpdateBetValue(int betAmount)
    {
        int currentBalance = PlayerPrefs.GetInt("Balance");
        _gameConfig.BetBase = betAmount > currentBalance ? currentBalance : betAmount;
    }
    
    private void DeductBet(int arg1, List<BettingCellPresenter> selectedCellsPresenter)
    {
        var currentBalance = PlayerPrefs.GetInt(BALANCE);
        var newBalance = currentBalance - _gameConfig.BetBase;
        PlayerPrefs.SetInt(BALANCE, newBalance);
    }

    public void Dispose()
    {
        _gameplayManager.DicesDistributed -= DeductBet;
        _gameplayManager.DicesDistributed -= UpdateBank;
    }
}