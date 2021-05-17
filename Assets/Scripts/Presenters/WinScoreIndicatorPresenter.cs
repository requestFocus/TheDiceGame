using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class WinScoreIndicatorPresenter : MonoBehaviour
{
    private GameManager _gameManager;
    
#pragma warning disable
    [SerializeField] private TextMeshProUGUI _totalScoreText;
    [SerializeField] private Image _winIndicator;
#pragma warning restore
    
    [Inject]
    private void Construct(GameManager gameManager)
    {
        _gameManager = gameManager;
    }
    
    private void Start()
    {
        _gameManager.TossingDicesCompleted += UpdateWinScoreIndicator;
        
        _totalScoreText.transform.localScale = Vector3.zero;
    }

    private void UpdateWinScoreIndicator(int totalScore, int leftSliderValue, int rightSliderValue)
    {
        UpdateWinIndicatorColor(totalScore, leftSliderValue, rightSliderValue);
        AnimateTotalScoreUpdate(totalScore, leftSliderValue, rightSliderValue);
    }

    private void UpdateWinIndicatorColor(int totalScore, int leftSliderValue, int rightSliderValue)
    {
        bool isWin = _gameManager.IsWin(leftSliderValue, rightSliderValue, totalScore);
        _winIndicator.color = isWin ? new Color(0, 255, 0, 0.5f) : new Color(255, 0, 0, 0.5f);
    }
    
    private void UpdateTotalScoreColor(int totalScore, int leftSliderValue, int rightSliderValue)
    {
        bool isWin = _gameManager.IsWin(totalScore, leftSliderValue, rightSliderValue);
        _totalScoreText.color = isWin ? new Color(0, 0.3f, 0, 0.25f) : new Color(0.3f, 0, 0, 0.25f);
    }

    private void UpdateTotalScoreText(int totalScore)
    {
        _totalScoreText.text = totalScore.ToString();
    }

    private void AnimateTotalScoreUpdate(int totalScore, int leftSliderValue, int rightSliderValue)
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(_totalScoreText.transform.DOScale(Vector3.zero, 0.1f))
            .InsertCallback(0.1f, () =>
            {
                UpdateTotalScoreText(totalScore);
                UpdateTotalScoreColor(totalScore, leftSliderValue, rightSliderValue);
            })
            .Insert(0.1f, _totalScoreText.transform.DOScale(Vector3.one, 0.1f))
            .Play();
    }

    private void OnDestroy()
    {
        _gameManager.TossingDicesCompleted -= UpdateWinScoreIndicator;
    }
}