using TMPro;

public class BettingSlider
{
    private int _leftSliderValue;
    private int _rightSliderValue;

    public void UpdateSliderValues(int leftSliderValue, int rightSliderValue)
    {
        _leftSliderValue = leftSliderValue;
        _rightSliderValue = rightSliderValue;
    }

    public int GetLeftSliderValue()
    {
        return _leftSliderValue;
    }

    public int GetRightSliderValue()
    {
        return _rightSliderValue;
    }
}