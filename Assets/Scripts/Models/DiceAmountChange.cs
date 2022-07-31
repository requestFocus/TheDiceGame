
public class DiceAmountChange
{
    private GameConfig _gameConfig;
    private GameplayManager _gameplayManager;

    public bool CanAddDice => _gameConfig.AmountOfDices < _gameConfig.MaxAmountOfDices;
    public bool CanRemoveDice => _gameConfig.AmountOfDices > _gameConfig.MinAmountOfDices;

    private DiceAmountChange(GameConfig gameConfig, GameplayManager gameplayManager)
    {
        _gameConfig = gameConfig;
        _gameplayManager = gameplayManager;
    }
    
    public void AddDice()
    {
        _gameConfig.AmountOfDices += 1;
        _gameplayManager.OnDiceAmountChanged();
    }

    public void RemoveDice()
    {
        _gameConfig.AmountOfDices -= 1;
        _gameplayManager.OnDiceAmountChanged();
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