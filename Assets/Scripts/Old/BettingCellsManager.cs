using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class BettingCellsManager : IInitializable, IDisposable
{
    // private BettingCellPresenter.Factory _cellPresenterFactory;
    // private GameConfig _gameConfig;

    // private readonly List<BettingCellPresenter> _cellPresenters = new List<BettingCellPresenter>();
    // private readonly List<BettingCellPresenter> _selectedCellPresenters = new List<BettingCellPresenter>();

    // private BettingCellPresenter _winningBettingCell;
    
    [Inject]
    private void Construct(BettingCellPresenter.Factory cellPresenterFactory, GameConfig gameConfig)
    {
        // _cellPresenterFactory = cellPresenterFactory;
        // _gameConfig = gameConfig;
    }

    public void Initialize()
    {
        // foreach (var cell in _cellPresenters)
        // {
        //     Action subscribeToValidateCell = () => { ValidateCell(cell); };
        //     cell.SelectableButton.OnEnter += subscribeToValidateCell;
        // }
        //
        // RefreshCellsButtonsInteractability();
        // ClearSelectedCells();
        //
        // _gameplayManager.DiceAmountChanged += UnmarkWinningCell;
        // _gameplayManager.DiceAmountChanged += ClearSelectedCells;
        // _gameplayManager.DiceAmountChanged += RefreshCellsButtonsInteractability;

        // _gameplayManager.DicesDistributed += MarkWinningCell;
    }

    // public void CreateCellPresenter(int index)
    // {
    //     BettingCellPresenter bettingCellPresenter = _cellPresenterFactory.Create(index);
    //     
    //     _cellPresenters.Add(bettingCellPresenter);
    // }

    // public List<BettingCellPresenter> GetCellPresenters()
    // {
    //     return _cellPresenters;
    // }

    // public List<BettingCellPresenter> GetSelectedCellsPresenters()
    // {
        // return _selectedCellPresenters;
    // }

    // private void ValidateCell(BettingCellPresenter bettingCell)
    // {
    //     if (bettingCell.IsSelected() && bettingCell.SelectableButton.interactable)
    //     {
    //         bettingCell.ChangeState();
    //         _selectedCellPresenters.Remove(bettingCell);
    //         _gameplayManager.OnCellTapped();
    //     }
    //     else if (!bettingCell.IsSelected() 
    //              && _selectedCellPresenters.Count < _gameConfig.MaxSelectedCellsAmount
    //              && bettingCell.SelectableButton.interactable)
    //     {
    //        bettingCell.ChangeState();
    //        _selectedCellPresenters.Add(bettingCell);
    //        _gameplayManager.OnCellTapped();
    //     }
    // }

    // private void ClearSelectedCells()
    // {
    //     _selectedCellPresenters.Clear();
    //     foreach (var cell in _cellPresenters)
    //     {
    //         cell.DeselectCell();
    //     }
    // }

    // private void RefreshCellsButtonsInteractability()
    // {
    //     foreach (var cell in _cellPresenters)
    //     {
    //         var isWithinPlayableRange = cell.GetCellId() >= _gameConfig.GetLowestPossibleBet - 1
    //                                     && cell.GetCellId() <= _gameConfig.GetHighestPossibleBet - 1;
    //         if (!isWithinPlayableRange)
    //         {
    //             cell.SetAsDisabled();
    //             cell.DisableInteractability();
    //         }
    //         else
    //         {
    //             cell.SetAsEnabled();
    //             cell.EnableInteractability();
    //         }
    //     }
    // }

    // private void MarkWinningCell(int totalScore, List<BettingCellPresenter> selectedCellPresenters)
    // {
        // _winningBettingCell = _cellPresenters[totalScore - 1];
        // _winningBettingCell.WinningIndicator.gameObject.SetActive(true);
        // _winningBettingCell.CellIdText.color = new Color(0, 0, 1, 1f);

        // _winningBettingCell.transform.DOScale(1.2f, 0.2f);
    // }
    
    // public void UnmarkWinningCell()
    // {
    //     if (_winningBettingCell != null && _winningBettingCell.SelectableButton.interactable)
    //     {
    //         _winningBettingCell.WinningIndicator.gameObject.SetActive(false);
    //         _winningBettingCell.CellIdText.color = Color.black;
    //         _winningBettingCell.transform.DOScale(Vector3.one, 0.2f);
    //     }
    // }

    public void Dispose()
    {
        // foreach (var cell in _cellPresenters)
        // {
        //     Action unsubscribeFromValidateCell = () => { ValidateCell(cell); };
        //     cell.SelectableButton.OnEnter -= unsubscribeFromValidateCell;
        // }
        
        // _gameplayManager.DiceAmountChanged -= RefreshCellsButtonsInteractability;
        // _gameplayManager.DiceAmountChanged -= ClearSelectedCells;
        
        // _gameplayManager.DicesDistributed -= MarkWinningCell;
    }
}