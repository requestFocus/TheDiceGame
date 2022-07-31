using System.Collections.Generic;

public class BettingSystem
{
    private readonly GameConfig _gameConfig;
    private readonly BettingCellsManager _bettingCellsManager;

    private BettingSystem(GameConfig gameConfig, BettingCellsManager bettingCellsManager)
    {
        _gameConfig = gameConfig;
        _bettingCellsManager = bettingCellsManager;
    }

    private int GetMaxAmountOfCells()
    {
        return _gameConfig.MaxAmountOfDices * _gameConfig.DiceSidesAmount;
    }
    
    public void CreateCellPresenters()
    {
        var amountOfCells = GetMaxAmountOfCells();
        for (int i = 0; i < amountOfCells; i++)
        {
            _bettingCellsManager.CreateCellPresenter(i);
        }
    }

    public List<BettingCellPresenter> GetCellPresenters()
    {
        return _bettingCellsManager.GetCellPresenters();
    }
}