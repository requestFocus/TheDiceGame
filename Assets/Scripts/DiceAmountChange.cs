using System;

public class DiceAmountChange
{
    private GameConfig _gameConfig;
    private GameManager _gameManager;

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
}