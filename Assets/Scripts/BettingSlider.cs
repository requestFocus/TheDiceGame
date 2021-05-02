using System;
using Min_Max_Slider;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class BettingSlider : MonoBehaviour
{
    [Inject] private GameConfig _gameConfig;
    [Inject] private DiceAmountChange _diceAmountChange;
    
    [SerializeField] private MinMaxSlider _slider;
    [SerializeField] private Image _fillDot;
    [SerializeField] private Image _winningDot;
    [SerializeField] private RectTransform _backgroundRectTransform;

    public float SliderLeftValue => _slider.Values.minValue;
    public float SliderRightValue => _slider.Values.maxValue;

    private void Start()
    {
        _slider.onValueChanged.AddListener(ValidateNoRangeDot);

        UpdateSlider();
        ValidateNoRangeDot(SliderLeftValue, SliderRightValue);

        _diceAmountChange.DiceAmountChanged += () =>
        {
            UpdateSlider();
            DisableWinningDot();
        }; 
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
        var doesMinEqualMax = (int)leftValue == (int)rightValue;
        _fillDot.gameObject.SetActive(doesMinEqualMax);
    }

    public void UpdateWinningDot(int totalScore)
    {
        _winningDot.gameObject.SetActive(true);
        Rect rect = _backgroundRectTransform.rect;
        RectTransform rectTransform = _winningDot.rectTransform;
        
        float xPositionOffset = rect.width / (_gameConfig.GetMaxSliderValue - _gameConfig.AmountOfDices) 
                        * (totalScore - _gameConfig.AmountOfDices) - rect.width / 2;
        rectTransform.localPosition = new Vector2(xPositionOffset, 0);
        
        float xPivotOffset =  rect.width / (_gameConfig.GetMaxSliderValue - _gameConfig.AmountOfDices) 
                           * (totalScore - _gameConfig.AmountOfDices) / rect.width;
        rectTransform.pivot = new Vector2(xPivotOffset, rectTransform.pivot.y);
    }

    public void DisableWinningDot()
    {
        _winningDot.gameObject.SetActive(false);
    }

    // private void OnDestroy()
    // {
    //     _diceAmountChange.DiceAmountChanged -= () =>
    //     {
    //         UpdateSlider();
    //         DisableWinningDot();
    //     };  
    // }
}