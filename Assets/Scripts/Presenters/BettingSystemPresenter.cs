using UnityEngine;
using Zenject;

public class BettingSystemPresenter : MonoBehaviour
{
    private CellsManager _cellsManager;
    private BettingSystem _model;
    
    [Inject]
    private void Construct(CellsManager cellsManager, BettingSystem model)
    {
        _cellsManager = cellsManager;
        _model = model;
    }

    private void Awake()
    {
        CreateCellPresenters();
        AssignCellsToParentTransform();
    }

    private void CreateCellPresenters()
    {
        var amountOfCells = _model.GetMaxAmountOfCells();
        for (int i = 0; i < amountOfCells; i++)
        {
            _cellsManager.CreateCellPresenter(i);
        }
    }

    private void AssignCellsToParentTransform()
    {
        foreach (var cell in _cellsManager.GetCellsPresenters())
        {
            Transform cellTransform = cell.transform;
            cellTransform.SetParent(transform);
            cellTransform.localScale = Vector3.one;;
        }
    }
}