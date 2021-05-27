using System;

public class DiceAmountChange
{
    private GameConfig _gameConfig;
    private GameManager _gameManager;

    public bool CanAddDice => _gameConfig.AmountOfDices < _gameConfig.MaxAmountOfDices;
    public bool CanRemoveDice => _gameConfig.AmountOfDices > _gameConfig.MinAmountOfDices;

    private DiceAmountChange(GameConfig gameConfig, GameManager gameManager)
    {
        _gameConfig = gameConfig;
        _gameManager = gameManager;
    }
    
    public void AddDice()
    {
        _gameConfig.AmountOfDices += 1;
        _gameManager.OnDiceAmountChanged();
    }

    public void RemoveDice()
    {
        _gameConfig.AmountOfDices -= 1;
        _gameManager.OnDiceAmountChanged();
    }

    public int GetCurrentAmountOfDices()
    {
        return _gameConfig.AmountOfDices;
    }

    public int GetMaxAmountOfDices()
    {
        return _gameConfig.MaxAmountOfDices;
    }

    public int GetMinAmountOfDices()
    {
        return _gameConfig.MinAmountOfDices;
    }
}