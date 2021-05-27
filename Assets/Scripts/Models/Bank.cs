using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class Bank : IInitializable, IDisposable
{
    private GameConfig _gameConfig;
    private GameManager _gameManager;

    private Bank(GameConfig gameConfig, GameManager gameManager)
    {
        _gameConfig = gameConfig;
        _gameManager = gameManager;
    }
    
    public void Initialize()
    {
        _gameManager.DicesDistributed += DeductBet;
        _gameManager.DicesDistributed += UpdateBank;
    }
    
    private void UpdateBank(int dicesSum, List<CellPresenter> selectedCellsPresenters)
    {
        var currentBalance = PlayerPrefs.GetInt("Balance", 200);
        var newBalance = currentBalance + CalculateOutcome(dicesSum, selectedCellsPresenters);
        PlayerPrefs.SetInt("Balance", newBalance);
    }

    public int CalculateOutcome(int dicesSum, List<CellPresenter> selectedCellPresenters)
    {
        if (!selectedCellPresenters.Select(cell => cell.GetCellId()).Contains(dicesSum - 1))
        {
            return 0;
        }

        return CalculatePotentialOutcome(selectedCellPresenters);
    }

    public int CalculatePotentialOutcome(List<CellPresenter> selectedCellPresenters)
    {
        return GetMultiplier(selectedCellPresenters) * _gameConfig.BetBase * _gameConfig.AmountOfDices;
    }
    
    public int GetMultiplier(List<CellPresenter> selectedCellPresenters)
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
        return PlayerPrefs.GetInt("Balance");
    }

    public int GetCurrentBet()
    {
        return _gameConfig.BetBase;
    }

    public void UpdateBetValue(int betAmount)
    {
        if (betAmount > PlayerPrefs.GetInt("Balance"))
        {
            betAmount = PlayerPrefs.GetInt("Balance");
        }
        
        _gameConfig.BetBase = betAmount;
    }
    
    private void DeductBet(int arg1, List<CellPresenter> selectedCellsPresenter)
    {
        var currentBalance = PlayerPrefs.GetInt("Balance");
        var newBalance = currentBalance - _gameConfig.BetBase;
        PlayerPrefs.SetInt("Balance", newBalance);
    }

    public void Dispose()
    {
        _gameManager.DicesDistributed -= DeductBet;
        _gameManager.DicesDistributed -= UpdateBank;
    }
}