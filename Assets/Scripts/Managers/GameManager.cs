using System;
using Zenject;

public class GameManager
{
    public event Action<int, int, int> TossingDicesCompleted;
    public event Action DicesTossed;
    public event Action<int, int> SliderUpdated;
    public event Action DiceAmountChanged;

#pragma warning disable CS0649
    [Inject] private GameConfig _gameConfig;
#pragma warning restore CS0649        
    
    public bool IsWin(int totalScore, int leftSliderValue, int rightSliderValue)
    {
        if (totalScore >= leftSliderValue && totalScore <= rightSliderValue)
        {
            return true;
        }

        return false;
    }

    public bool IsWithinWiningRange(int leftSliderValue, int rightSliderValue)
    {
        return rightSliderValue - leftSliderValue <= _gameConfig.WidestWinningRange;
    }

    public void OnDicesToss(int totalScore, int leftSliderValue, int rightSliderValue)
    {
        TossingDicesCompleted?.Invoke(totalScore, leftSliderValue, rightSliderValue);
    }

    public void OnTurnEnd()
    {
        DicesTossed?.Invoke();
    } 
    
    public void OnSliderUpdated(int leftSliderValue, int rightSliderValue)
    {
        SliderUpdated?.Invoke(leftSliderValue, rightSliderValue);
    }

    public void OnDiceAmountChanged()
    {
        DiceAmountChanged?.Invoke();
    }
}