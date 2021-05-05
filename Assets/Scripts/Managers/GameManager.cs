using System;

public class GameManager
{
    public event Action<int, int, int> DicesTossed;
        
    public bool IsWin(int leftSliderValue, int rightSliderValue, int totalScore)
    {
        if (totalScore >= leftSliderValue && totalScore <= rightSliderValue)
        {
            return true;
        }

        return false;
    }

    public void OnDicesTossed(int leftSliderValue, int rightSliderValue, int totalScore)
    {
        DicesTossed?.Invoke(leftSliderValue, rightSliderValue, totalScore);
    }
}