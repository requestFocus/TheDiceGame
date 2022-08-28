using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class Bank : IInitializable, IDisposable
{
    private GameConfig _gameConfig;
    
    private const string BALANCE = "Balance";
    
    public event Action<List<BettingCellPresenter>> CellTappedRedirect;
    public event Action<List<BettingCellPresenter>> DiceAmountChangedRedirect;
    public event Action<int, List<BettingCellPresenter>> DicesDistributedRedirect;

    public event Func<List<BettingCellPresenter>> SliderUpdated;

    private Bank(GameConfig gameConfig)
    {
        _gameConfig = gameConfig;
    }
    
    public void Initialize()
    {
        DicesDistributedRedirect += DeductBet;
        DicesDistributedRedirect += UpdateBank;
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

    public void UpdatePotentialWin(List<BettingCellPresenter> selectedCellPresenters)
    {
        CellTappedRedirect?.Invoke(selectedCellPresenters);
    }
    
    public void DiceAmountChanged(List<BettingCellPresenter> selectedCellPresenters)
    {
        DiceAmountChangedRedirect?.Invoke(selectedCellPresenters);
    }

    public void UpdatePostTossOutcome(int totalScore, List<BettingCellPresenter> selectedCellPresenter)
    {
        DicesDistributedRedirect?.Invoke(totalScore, selectedCellPresenter);
    }

    public void Dispose()
    {
        DicesDistributedRedirect -= DeductBet;
        DicesDistributedRedirect -= UpdateBank;
    }

    public List<BettingCellPresenter> GetSelectedCellsPresenters()
    {
        return SliderUpdated?.Invoke();
    }
}