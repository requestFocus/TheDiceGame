using System;
using Min_Max_Slider;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class BettingSliderPresenter : MonoBehaviour
{
    private GameConfig _gameConfig;
    private GameManager _gameManager;
    private BettingSlider _model;

#pragma warning disable
    [SerializeField] private MinMaxSlider _slider;
    [SerializeField] private Image _fillDot;
    [SerializeField] private Image _winningDot;
    [SerializeField] private RectTransform _backgroundRectTransform;
#pragma warning restore
    
    [Inject]
    private void Construct(GameConfig gameConfig, GameManager gameManager, BettingSlider model)
    {
        _gameConfig = gameConfig;
        _gameManager = gameManager;
        _model = model;
    }

    private void Start()
    {
        _slider.onValueChanged.AddListener(ValidateNoRangeDot);
        _slider.onValueChanged.AddListener(UpdateBank);
        _slider.onValueChanged.AddListener(UpdateSliderValues);

        UpdateSlider();
        ValidateNoRangeDot(_model.GetLeftSliderValue(), _model.GetRightSliderValue());

        _gameManager.DiceAmountChanged += PrepareSliderAfterDiceAmountChange;
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

    private void UpdateBank(float leftValue, float rightValue)
    {
        // _gameManager.OnSliderUpdated((int)leftValue, (int)rightValue);
    }
    
    private void UpdateSliderValues(float leftValue, float rightValue)
    {
        _model.UpdateSliderValues((int)leftValue, (int)rightValue);
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
        _gameManager.DiceAmountChanged -= PrepareSliderAfterDiceAmountChange;
    }
}