using System;

public class DiceAmountChange
{
    private GameConfig _gameConfig;

    private DiceAmountChange(GameConfig gameConfig)
    {
        _gameConfig = gameConfig;
    }
    
    public event Action DiceAmountChanged;

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
}