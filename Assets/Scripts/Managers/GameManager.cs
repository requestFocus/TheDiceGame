public class GameManager
{
    public bool ValidateBet(int leftSliderValue, int rightSliderValue, int totalScore)
    {
        if (totalScore >= leftSliderValue && totalScore <= rightSliderValue)
        {
            return true;
        }

        return false;
    }
}