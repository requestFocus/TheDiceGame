using System.Collections.Generic;
using System.Linq;
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
        _gameManager.DicesDistributed += UpdateWinScoreIndicator;
        _gameManager.TurnStarted += HideWinScoreIndicator;

        _totalScoreText.transform.localScale = Vector3.zero;
    }

    private void UpdateWinScoreIndicator(int totalScore, List<CellPresenter> selectedCellsPresenter)
    {
        UpdateWinIndicatorColor(totalScore, selectedCellsPresenter);
        AnimateTotalScoreUpdate(totalScore, selectedCellsPresenter);
    }

    private void UpdateWinIndicatorColor(int totalScore, List<CellPresenter> selectedCellsPresenter)
    {
        bool isWin = IsWin(totalScore, selectedCellsPresenter);
        _winIndicator.color = isWin ? new Color(0, 255, 0, 0.5f) : new Color(255, 0, 0, 0.5f);
    }
    
    private void UpdateTotalScoreColor(int totalScore, List<CellPresenter> selectedCellsPresenter)
    {
        bool isWin = IsWin(totalScore, selectedCellsPresenter);
        _totalScoreText.color = isWin ? new Color(0, 0.3f, 0, 0.25f) : new Color(0.3f, 0, 0, 0.25f);
    }
    
    private bool IsWin(int totalScore, List<CellPresenter> selectedCellsPresenters)
    {
        return selectedCellsPresenters.Select(cell => cell.GetCellId()).Contains(totalScore - 1);
    }

    private void UpdateTotalScoreText(int totalScore)
    {
        _totalScoreText.text = totalScore.ToString();
    }

    private void AnimateTotalScoreUpdate(int totalScore, List<CellPresenter> selectedCellsPresenter)
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(_totalScoreText.transform.DOScale(Vector3.zero, 0.1f))
            .InsertCallback(0.1f, () =>
            {
                UpdateTotalScoreText(totalScore);
                UpdateTotalScoreColor(totalScore, selectedCellsPresenter);
            })
            .Insert(0.1f, _totalScoreText.transform.DOScale(Vector3.one, 0.1f))
            .Play();
    }

    private void HideWinScoreIndicator()
    {
        _totalScoreText.text = "";
    }

private void OnDestroy()
    {
        _gameManager.DicesDistributed -= UpdateWinScoreIndicator;
        _gameManager.TurnStarted -= HideWinScoreIndicator;
    }
}