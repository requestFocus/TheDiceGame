using System;
using System.Collections.Generic;

public class GameplayManager
{
    // private readonly BettingCellsManager _bettingCellsManager;
    // private readonly DicesManager _dicesManager;

    public GameplayManager()//BettingCellsManager bettingCellsManager, DicesManager dicesManager)
    {
        // _bettingCellsManager = bettingCellsManager;
        // _dicesManager = dicesManager;
    }

    public event Action<int, List<BettingCellPresenter>> DicesDistributed;
    public event Action<List<BettingCellPresenter>> CellTapped;
    public event Action DiceAmountChanged;

    public void OnDicesDistributed()
    {
        // DicesDistributed?.Invoke(_dicesManager.GetDicesSum(), _bettingCellsManager.GetSelectedCellsPresenters());
    }

    public void OnDiceAmountChanged()
    {
        // DiceAmountChanged?.Invoke();
    }

    public void OnCellTapped()
    {
        // CellTapped?.Invoke(_bettingCellsManager.GetSelectedCellsPresenters());
    }

    // public List<BettingCellPresenter> GetSelectedCellsPresenters()
    // {
        // return _bettingCellsManager.GetSelectedCellsPresenters();
    // }
}