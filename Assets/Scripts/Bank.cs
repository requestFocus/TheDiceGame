using System;
using UnityEngine;
using Zenject;

public class Bank : IInitializable, IDisposable
{
    private GameConfig _gameConfig;
    private GameManager _gameManager;

    public event Action BankUpdated;

    private Bank(GameConfig gameConfig, GameManager gameManager)
    {
        _gameConfig = gameConfig;
        _gameManager = gameManager;
    }
    
    public void Initialize()
    {
        _gameManager.DicesTossed += MakeBet;
        _gameManager.DicesTossed += UpdateBank;
        
        BankUpdated?.Invoke();
    }
    
    private void UpdateBank(int leftBet, int rightBet, int dicesSum)
    {
        var currentBalance = PlayerPrefs.GetInt("Balance", 200);
        var newBalance = currentBalance + CalculateOutcome(leftBet, rightBet, dicesSum);
        PlayerPrefs.SetInt("Balance", newBalance);
    }

    public int CalculateOutcome(int leftBet, int rightBet, int dicesSum)
    {
        if (dicesSum < leftBet || dicesSum > rightBet)
        {
            return 0;
        }

        int range = rightBet - leftBet;

        switch (range)
        {
            case 0:
                return _gameConfig.BetBase * _gameConfig.WinMultiplierForRangeZero * _gameConfig.AmountOfDices;
            case 1:
                return _gameConfig.BetBase * _gameConfig.WinMultiplierForRangeOne * _gameConfig.AmountOfDices;
            case 2:
                return _gameConfig.BetBase * _gameConfig.WinMultiplierForRangeTwo * _gameConfig.AmountOfDices;
            case 3:
                return _gameConfig.BetBase * _gameConfig.WinMultiplierForRangeThree * _gameConfig.AmountOfDices;
            default:
                return 0;
        } 
    }

    public int GetBalance()
    {
        return PlayerPrefs.GetInt("Balance");
    }
    
    private void MakeBet(int arg1, int arg2, int arg3)
    {
        var currentBalance = PlayerPrefs.GetInt("Balance", 200);
        var newBalance = currentBalance - _gameConfig.BetBase;
        PlayerPrefs.SetInt("Balance", newBalance);
    }

    public void Dispose()
    {
        _gameManager.DicesTossed -= MakeBet;
        _gameManager.DicesTossed -= UpdateBank;
    }
}