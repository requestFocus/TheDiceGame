using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CellsManager2 : IInitializable, IDisposable
{
    private CellPresenter2.Factory _cellPresenterFactory;

    private List<CellPresenter2> _cellPresenters = new List<CellPresenter2>();
    private List<CellPresenter2> _selectedCellPresenters = new List<CellPresenter2>();

    private Color _cellColor;
    
    [Inject]
    private void Construct(CellPresenter2.Factory cellPresenterFactory)
    {
        _cellPresenterFactory = cellPresenterFactory;
    }

    public void Initialize()
    {
        foreach (var cell in _cellPresenters)
        {
            cell.SelectableButton.OnEnter += () =>
            {
                Debug.Log("enter");
                ValidateCell(cell);
            };
        }
    }

    public void CreateCellPresenter(int index)
    {
        CellPresenter2 cellPresenter = _cellPresenterFactory.Create();
        cellPresenter.SetCellId(index);

        _cellPresenters.Add(cellPresenter);
    }

    public List<CellPresenter2> GetCellsPresenters()
    {
        return _cellPresenters;
    }

    private void ValidateCell(CellPresenter2 cell)
    {
        if (cell.GetSelectedState())
        {
            cell.ChangeState(_cellColor);
            _selectedCellPresenters.Remove(cell);
        }
        else if (!cell.GetSelectedState())
        {
            cell.ChangeState(_cellColor);
            _selectedCellPresenters.Add(cell);
        }
    }

    public void Dispose()
    {
        foreach (var cell in _cellPresenters)
        {
            void OnEnterEventUnsubscribe()
            {
                Debug.Log("enter");
                ValidateCell(cell);
            }

            cell.SelectableButton.OnEnter -= OnEnterEventUnsubscribe;
        }
    }

    public void SetCellColor(Color color)
    {
        _cellColor = color;
    }

    public Color GetCellColor()
    {
        return _cellColor;
    }
}