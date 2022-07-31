using System;
using Min_Max_Slider;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class BettingSliderPresenter : MonoBehaviour
{
    private GameConfig _gameConfig;
    private GameplayManager _gameplayManager;
    private BettingSlider _model;

#pragma warning disable
    [SerializeField] private MinMaxSlider _slider;
    [SerializeField] private Image _fillDot;
    [SerializeField] private Image _winningDot;
#pragma warning restore
    
    [Inject]
    private void Construct(GameConfig gameConfig, GameplayManager gameplayManager, BettingSlider model)
    {
        _gameConfig = gameConfig;
        _gameplayManager = gameplayManager;
        _model = model;
    }

    private void Start()
    {
        _slider.onValueChanged.AddListener(ValidateNoRangeDot);
        _slider.onValueChanged.AddListener(UpdateSliderValues);

        UpdateSlider();
        ValidateNoRangeDot(_model.GetLeftSliderValue(), _model.GetRightSliderValue());

        _gameplayManager.DiceAmountChanged += PrepareSliderAfterDiceAmountChange;
    }

    private void UpdateSlider()
    {
        float minSliderValue = _gameConfig.AmountOfDices;
        float maxSliderValue = _gameConfig.GetMaxSliderValue;
        _slider.SetLimits(minSliderValue, maxSliderValue);
        _slider.SetValues((maxSliderValue - minSliderValue) / 2, (maxSliderValue - minSliderValue) / 2 + 2);
    }

    private void ValidateNoRangeDot(float leftValue, float rightValue)
    {
        var doesMinEqualMax = (int) leftValue == (int) rightValue;
        _fillDot.gameObject.SetActive(doesMinEqualMax);
    }

    private void UpdateSliderValues(float leftValue, float rightValue)
    {
        _model.UpdateSliderValues((int)leftValue, (int)rightValue);
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
        _gameplayManager.DiceAmountChanged -= PrepareSliderAfterDiceAmountChange;
    }
}