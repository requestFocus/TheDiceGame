public class BettingSystem
{
    private readonly GameConfig _gameConfig;

    private BettingSystem(GameConfig gameConfig)
    {
        _gameConfig = gameConfig;
    }

    public int GetMaxAmountOfCells()
    {
        return _gameConfig.MaxAmountOfDices * _gameConfig.DiceSidesAmount;
    }
}