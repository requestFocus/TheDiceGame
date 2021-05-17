using System;
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
        _gameManager.TossingDicesCompleted += DeductBet;
        _gameManager.TossingDicesCompleted += UpdateBank;
    }
    
    private void UpdateBank(int dicesSum, int leftBet, int rightBet)
    {
        var currentBalance = PlayerPrefs.GetInt("Balance", 200);
        var newBalance = currentBalance + CalculateOutcome(dicesSum, leftBet, rightBet);
        PlayerPrefs.SetInt("Balance", newBalance);
    }

    public int CalculateOutcome(int dicesSum, int leftBet, int rightBet)
    {
        if (dicesSum < leftBet || dicesSum > rightBet)
        {
            return 0;
        }

        return CalculatePotentialOutcome(leftBet, rightBet);
    }

    public int CalculatePotentialOutcome(int leftBet, int rightBet)
    {
        return GetMultiplier(leftBet, rightBet) * _gameConfig.BetBase * _gameConfig.AmountOfDices;
    }
    
    public int GetMultiplier(int leftBet, int rightBet)
    {
        int range = rightBet - leftBet;

        switch (range)
        {
            case 0:
                return _gameConfig.WinMultiplierForRangeZero;
            case 1:
                return _gameConfig.WinMultiplierForRangeOne;
            case 2:
                return _gameConfig.WinMultiplierForRangeTwo;
            case 3:
                return _gameConfig.WinMultiplierForRangeThree;
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
    
    private void DeductBet(int arg1, int arg2, int arg3)
    {
        var currentBalance = PlayerPrefs.GetInt("Balance");
        var newBalance = currentBalance - _gameConfig.BetBase;
        PlayerPrefs.SetInt("Balance", newBalance);
    }

    public void Dispose()
    {
        _gameManager.TossingDicesCompleted -= DeductBet;
        _gameManager.TossingDicesCompleted -= UpdateBank;
    }
}