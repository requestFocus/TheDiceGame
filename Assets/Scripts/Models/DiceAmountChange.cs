
using System;

public class DiceAmountChange
{
    private GameConfig _gameConfig;

    public bool CanAddDice => _gameConfig.AmountOfDices < _gameConfig.MaxAmountOfDices;
    public bool CanRemoveDice => _gameConfig.AmountOfDices > _gameConfig.MinAmountOfDices;
    
    public event Action DiceAmountChanged;

    private DiceAmountChange(GameConfig gameConfig)
    {
        _gameConfig = gameConfig;
    }
    
    public void AddDice()
    {
        _gameConfig.AmountOfDices += 1;
        DiceAmountChanged?.Invoke();
    }

    public void RemoveDice()
    {
        _gameConfig.AmountOfDices -= 1;
        DiceAmountChanged?.Invoke();
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