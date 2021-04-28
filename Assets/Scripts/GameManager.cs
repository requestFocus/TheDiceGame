public class GameManager
{
    public bool ValidateBet(int sliderValue, int totalScore)
    {
        if (sliderValue == totalScore)
        {
            return true;
        }

        return false;
    }
}