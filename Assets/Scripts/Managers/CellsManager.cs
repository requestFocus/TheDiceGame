using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class CellsManager : IInitializable, IDisposable
{
    private CellPresenter.Factory _cellPresenterFactory;
    private GameConfig _gameConfig;
    private GameManager _gameManager;

    private List<CellPresenter> _cellPresenters = new List<CellPresenter>();
    private List<CellPresenter> _selectedCellPresenters = new List<CellPresenter>();

    private CellPresenter _winningCell;
    
    [Inject]
    private void Construct(CellPresenter.Factory cellPresenterFactory, GameConfig gameConfig,
        GameManager gameManager)
    {
        _cellPresenterFactory = cellPresenterFactory;
        _gameConfig = gameConfig;
        _gameManager = gameManager;
    }

    public void Initialize()
    {
        foreach (var cell in _cellPresenters)
        {
            cell.Button.onClick.AddListener(() =>
            {
                ValidateCell(cell);
            });
        }

        RefreshCellsButtonsInteractability();
        ClearSelectedCells();

        _gameManager.DiceAmountChanged += UnmarkWinningCell;
        _gameManager.DiceAmountChanged += ClearSelectedCells;
        _gameManager.DiceAmountChanged += RefreshCellsButtonsInteractability;

        _gameManager.DicesTossed += MarkWinningCell;
        _gameManager.TurnStarted += UnmarkWinningCell;
    }

    public void CreateCellPresenter(int index)
    {
        CellPresenter cellPresenter = _cellPresenterFactory.Create();
        cellPresenter.SetCellId(index);
        cellPresenter.SetCellIdText();
        
        _cellPresenters.Add(cellPresenter);
    }

    public List<CellPresenter> GetCellsPresenters()
    {
        return _cellPresenters;
    }

    public List<CellPresenter> GetSelectedCellsPresenters()
    {
        return _selectedCellPresenters;
    }

    private void ValidateCell(CellPresenter cell)
    {
        if (cell.GetSelectedState())
        {
            cell.ChangeState();
            _selectedCellPresenters.Remove(cell);
            _gameManager.OnCellTapped(_selectedCellPresenters);
        }
        else if (!cell.GetSelectedState() && _selectedCellPresenters.Count < _gameConfig.MaxSelectedCellsAmount)
        {
           cell.ChangeState();
           _selectedCellPresenters.Add(cell);
           _gameManager.OnCellTapped(_selectedCellPresenters);
        }
    }

    private void ClearSelectedCells()
    {
        _selectedCellPresenters.Clear();
        foreach (var cell in _cellPresenters)
        {
            cell.DeselectCell();
        }
    }

    private void RefreshCellsButtonsInteractability()
    {
        foreach (var cell in _cellPresenters)
        {
            var isWithinPlayableRange = cell.GetCellId() >= _gameConfig.GetLowestValueForCurrentDicesAmount - 1
                                        && cell.GetCellId() <= _gameConfig.GetHighestValueForCurrentDicesAmount - 1;
            if (!isWithinPlayableRange)
            {
                cell.SetAsDisabled();
                cell.DisableInteractability();
            }
            else
            {
                cell.SetAsEnabled();
                cell.EnableInteractability();
            }
        }
    }

    private void MarkWinningCell(int totalScore, List<CellPresenter> selectedCellPresenters)
    {
        _winningCell = _cellPresenters[totalScore - 1];
        _winningCell.WinningIndicator.gameObject.SetActive(true);
        _winningCell.CellIdText.color = new Color(0, 0, 1, 1f);

        _winningCell.transform.DOScale(1.2f, 0.2f);
    }

    private void UnmarkWinningCell()
    {
        if (_winningCell != null && _winningCell.Button.interactable)
        {
            _winningCell.WinningIndicator.gameObject.SetActive(false);
            _winningCell.CellIdText.color = Color.black;
            _winningCell.transform.DOScale(Vector3.one, 0.2f);
        }
    }

    public void Dispose()
    {
        foreach (var cell in _cellPresenters)
        {
            cell.Button.onClick.RemoveAllListeners();
        }
        
        _gameManager.DiceAmountChanged -= RefreshCellsButtonsInteractability;
        _gameManager.DiceAmountChanged -= ClearSelectedCells;
        
        _gameManager.DicesTossed -= MarkWinningCell;
        _gameManager.TurnStarted -= UnmarkWinningCell;
    }
}