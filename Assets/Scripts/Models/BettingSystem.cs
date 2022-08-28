using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class BettingSystem : IInitializable, IDisposable
{
    private readonly GameConfig _gameConfig;
    private readonly DiceAmountChange _diceAmountChange;
    private readonly BettingCellPresenter.Factory _cellPresenterFactory;

    private BettingCellPresenter _winningBettingCell;
    
    private readonly List<BettingCellPresenter> _cellPresenters = new List<BettingCellPresenter>();
    private readonly List<BettingCellPresenter> _selectedCellPresenters = new List<BettingCellPresenter>();

    public event Action<List<BettingCellPresenter>> CellTapped;

    private BettingSystem(GameConfig gameConfig, 
        DiceAmountChange diceAmountChange,
        BettingCellPresenter.Factory cellPresenterFactory)
    {
        _gameConfig = gameConfig;
        _diceAmountChange = diceAmountChange;
        _cellPresenterFactory = cellPresenterFactory;
    }
    
    public void Initialize()
    {
        _diceAmountChange.DiceAmountChanged += UnmarkWinningCell;
        _diceAmountChange.DiceAmountChanged += ClearSelectedCells;
        _diceAmountChange.DiceAmountChanged += RefreshCellsButtonsInteractability;
        
        foreach (var cell in _cellPresenters)
        {
            Action subscribeToValidateCell = () => { ValidateCell(cell); };
            cell.SelectableButton.OnEnter += subscribeToValidateCell;
        }

        RefreshCellsButtonsInteractability();
        ClearSelectedCells();
    }
    
    public void CreateCellPresenter(int index)
    {
        BettingCellPresenter bettingCellPresenter = _cellPresenterFactory.Create(index);
        
        _cellPresenters.Add(bettingCellPresenter);
    }

    private int GetMaxAmountOfCells()
    {
        return _gameConfig.MaxAmountOfDices * _gameConfig.DiceSidesAmount;
    }
    
    public void CreateCellPresenters()
    {
        var amountOfCells = GetMaxAmountOfCells();
        for (int i = 0; i < amountOfCells; i++)
        {
            CreateCellPresenter(i);
        }
    }

    public List<BettingCellPresenter> GetCellPresenters()
    {
        return _cellPresenters;
    }
    
    public List<BettingCellPresenter> GetSelectedCellsPresenters()
    {
        return _selectedCellPresenters;
    }
    
    private void ValidateCell(BettingCellPresenter bettingCell)
    {
        if (bettingCell.IsSelected() && bettingCell.SelectableButton.interactable)
        {
            bettingCell.ChangeState();
            _selectedCellPresenters.Remove(bettingCell);
            OnCellTapped();
        }
        else if (!bettingCell.IsSelected() 
                 && _selectedCellPresenters.Count < _gameConfig.MaxSelectedCellsAmount
                 && bettingCell.SelectableButton.interactable)
        {
            bettingCell.ChangeState();
            _selectedCellPresenters.Add(bettingCell);
            OnCellTapped();
        }
    }

    private void OnCellTapped()
    {
        CellTapped?.Invoke(GetSelectedCellsPresenters());
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
            var isWithinPlayableRange = cell.GetCellId() >= _gameConfig.GetLowestPossibleBet - 1
                                        && cell.GetCellId() <= _gameConfig.GetHighestPossibleBet - 1;
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
    
    public void MarkWinningCell(int totalScore, List<BettingCellPresenter> _)
    {
        _winningBettingCell = _cellPresenters[totalScore - 1];
        _winningBettingCell.WinningIndicator.gameObject.SetActive(true);
        _winningBettingCell.CellIdText.color = new Color(0, 0, 1, 1f);

        _winningBettingCell.transform.DOScale(1.2f, 0.2f);
    }

    public void UnmarkWinningCell()
    {
        if (_winningBettingCell != null && _winningBettingCell.SelectableButton.interactable)
        {
            _winningBettingCell.WinningIndicator.gameObject.SetActive(false);
            _winningBettingCell.CellIdText.color = Color.black;
            _winningBettingCell.transform.DOScale(Vector3.one, 0.2f);
        }
    }

    public void Dispose()
    {
        _diceAmountChange.DiceAmountChanged -= UnmarkWinningCell;
        _diceAmountChange.DiceAmountChanged -= ClearSelectedCells;
        _diceAmountChange.DiceAmountChanged -= RefreshCellsButtonsInteractability;
        
        foreach (var cell in _cellPresenters)
        {
            Action unsubscribeFromValidateCell = () => { ValidateCell(cell); };
            cell.SelectableButton.OnEnter -= unsubscribeFromValidateCell;
        }
    }
}