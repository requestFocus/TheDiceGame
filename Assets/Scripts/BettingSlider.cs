using System;
using Min_Max_Slider;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class BettingSlider : MonoBehaviour
{
    private GameConfig _gameConfig;
    private DiceAmountChange _diceAmountChange;

    [SerializeField] private MinMaxSlider _slider;
    [SerializeField] private Image _fillDot;
    [SerializeField] private Image _winningDot;
    [SerializeField] private RectTransform _backgroundRectTransform;

    [Inject]
    private void Construct(GameConfig gameConfig, DiceAmountChange diceAmountChange)
    {
        _gameConfig = gameConfig;
        _diceAmountChange = diceAmountChange;
    }

    public float SliderLeftValue => _slider.Values.minValue;
    public float SliderRightValue => _slider.Values.maxValue;

    private void Start()
    {
        _slider.onValueChanged.AddListener(ValidateNoRangeDot);

        UpdateSlider();
        ValidateNoRangeDot(SliderLeftValue, SliderRightValue);

        _diceAmountChange.DiceAmountChanged += PrepareSliderAfterDiceAmountChange;
    }

    private void UpdateSlider()
    {
        float minSliderValue = _gameConfig.AmountOfDices;
        float maxSliderValue = _gameConfig.GetMaxSliderValue;
        _slider.SetLimits(minSliderValue, maxSliderValue);
        _slider.SetValues(maxSliderValue / 2, maxSliderValue / 2 + 1);
    }

    private void ValidateNoRangeDot(float leftValue, float rightValue)
    {
        var doesMinEqualMax = (int) leftValue == (int) rightValue;
        _fillDot.gameObject.SetActive(doesMinEqualMax);
    }

    public void UpdateWinningDot(bool isWin, int totalScore)
    {
        _winningDot.gameObject.SetActive(true);
        Rect rect = _backgroundRectTransform.rect;
        RectTransform rectTransform = _winningDot.rectTransform;

        float xPositionOffset = rect.width / (_gameConfig.GetMaxSliderValue - _gameConfig.AmountOfDices)
                                * (totalScore - _gameConfig.AmountOfDices) - rect.width / 2;
        rectTransform.localPosition = new Vector2(xPositionOffset, rectTransform.localPosition.y);

        float xPivotOffset = rect.width / (_gameConfig.GetMaxSliderValue - _gameConfig.AmountOfDices)
                             * (totalScore - _gameConfig.AmountOfDices) / rect.width;
        rectTransform.pivot = new Vector2(xPivotOffset, rectTransform.pivot.y);

        _winningDot.color = isWin ? new Color(0, 255, 0, 1) : new Color(255, 0, 0, 1);
    }

    private void DisableWinningDot()
    {
        _winningDot.gameObject.SetActive(false);
    }

    private void PrepareSliderAfterDiceAmountChange()
    {
        UpdateSlider();
        DisableWinningDot();
    }

    private void OnDestroy()
    {
        _diceAmountChange.DiceAmountChanged -= PrepareSliderAfterDiceAmountChange;
    }
}