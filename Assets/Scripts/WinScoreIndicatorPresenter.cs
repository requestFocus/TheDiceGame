using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class WinScoreIndicatorPresenter : MonoBehaviour
{
    private GameManager _gameManager;
    
    [SerializeField] private TextMeshProUGUI _totalScoreText;
    [SerializeField] private Image _winIndicator;

    [Inject]
    private void Construct(GameManager gameManager)
    {
        _gameManager = gameManager;
    }
    
    private void Start()
    {
        gameObject.SetActive(false);

        _gameManager.DicesTossed += UpdateWinScoreIndicator;
    }

    private void UpdateWinScoreIndicator(int leftSliderValue, int rightSliderValue, int totalScore)
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }
        
        UpdateWinIndicatorColor(leftSliderValue, rightSliderValue, totalScore);
        UpdateTotalScoreText(totalScore);
    }

    private void UpdateWinIndicatorColor(int leftSliderValue, int rightSliderValue, int totalScore)
    {
        bool isWin = _gameManager.IsWin(leftSliderValue, rightSliderValue, totalScore);
        _winIndicator.GetComponent<Image>().color = isWin ? new Color(0, 255, 0, 0.5f) : new Color(255, 0, 0, 0.5f);
    }

    private void UpdateTotalScoreText(int totalScore)
    {
        _totalScoreText.text = totalScore.ToString();
    }

    private void OnDestroy()
    {
        _gameManager.DicesTossed -= UpdateWinScoreIndicator;
    }
}