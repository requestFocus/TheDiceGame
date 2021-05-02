using System;
using Zenject;

public class DiceAmountChange
{
    [Inject] private GameConfig _gameConfig;
    
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