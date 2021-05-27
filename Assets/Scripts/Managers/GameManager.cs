using System;
using System.Collections.Generic;

public class GameManager
{
    private readonly CellsManager _cellsManager;
    private readonly DicesManager _dicesManager;

    public GameManager(CellsManager cellsManager, DicesManager dicesManager)
    {
        _cellsManager = cellsManager;
        _dicesManager = dicesManager;
    }

    public event Action<int, List<CellPresenter>> DicesDistributed;
    public event Action<List<CellPresenter>> CellTapped;
    public event Action TurnStarted;
    public event Action TurnEnded;
    public event Action DiceAmountChanged;

    public void OnDicesDistributed()
    {
        DicesDistributed?.Invoke(_dicesManager.GetDicesSum(), _cellsManager.GetSelectedCellsPresenters());
    }

    public void OnTurnStart()
    {
        TurnStarted?.Invoke();
    }
    
    public void OnTurnEnd()
    {
        TurnEnded?.Invoke();
    }
    
    public void OnDiceAmountChanged()
    {
        DiceAmountChanged?.Invoke();
    }

    public void OnCellTapped()
    {
        CellTapped?.Invoke(_cellsManager.GetSelectedCellsPresenters());
    }

    public List<CellPresenter> GetSelectedCellsPresenters()
    {
        return _cellsManager.GetSelectedCellsPresenters();
    }
}