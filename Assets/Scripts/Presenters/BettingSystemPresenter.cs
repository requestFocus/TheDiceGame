using UnityEngine;
using Zenject;

public class BettingSystemPresenter : MonoBehaviour
{
    private GameConfig _gameConfig;
    private CellsManager _cellsManager;
    
    [Inject]
    private void Construct(GameConfig gameConfig, CellsManager cellsManager)
    {
        _gameConfig = gameConfig;
        _cellsManager = cellsManager;
    }

    private void Awake()
    {
        CreateCellPresenters();
        AssignCellsToParentTransform();
    }

    private void CreateCellPresenters()
    {
        var amountOfCells = _gameConfig.MaxAmountOfDices * _gameConfig.DiceSidesAmount;
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