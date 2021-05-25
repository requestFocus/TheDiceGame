using System;
using System.Collections.Generic;

public class GameManager
{
    public event Action<int, List<CellPresenter>> DicesTossed;
    public event Action TurnStarted;
    public event Action TurnEnded;
    public event Action DiceAmountChanged;
    public event Action<List<CellPresenter>> CellTapped;

    public void OnDicesToss(int totalScore, List<CellPresenter> selectedCellPresenters)
    {
        DicesTossed?.Invoke(totalScore, selectedCellPresenters);
    }

    public void OnTurnEnd()
    {
        TurnEnded?.Invoke();
    }

    public void OnTurnStart()
    {
        TurnStarted?.Invoke();
    }
    
    public void OnDiceAmountChanged()
    {
        DiceAmountChanged?.Invoke();
    }

    public void OnCellTapped(List<CellPresenter> selectedCellPresenters)
    {
        CellTapped?.Invoke(selectedCellPresenters);
    }
}