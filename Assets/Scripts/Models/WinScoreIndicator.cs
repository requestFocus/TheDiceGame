using System;
using System.Collections.Generic;
using Zenject;

public class WinScoreIndicator
{
    public event Action<int, List<BettingCellPresenter>> DicesDistributedRedirect;
    
    private WinScoreIndicator()
    {
    }

    public void UpdateWinScoreIndicator(int totalScore, List<BettingCellPresenter> selectedCellPresenters)
    {
        DicesDistributedRedirect?.Invoke(totalScore, selectedCellPresenters);
    }
}