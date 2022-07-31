using UnityEngine;
using Zenject;

public class BettingSystemPresenter : MonoBehaviour
{
    [SerializeField] private RectTransform _contentTransform; 
    
    private BettingSystem _model;
    
    [Inject]
    private void Construct(BettingCellsManager cellsManager, BettingSystem model)
    {
        _model = model;
    }

    private void Awake()
    {
        _model.CreateCellPresenters();
        AssignCellsToParentTransform();
    }

    private void AssignCellsToParentTransform()
    {
        foreach (var cell in _model.GetCellPresenters())
        {
            Transform cellTransform = cell.transform;
            cellTransform.SetParent(_contentTransform);
            cellTransform.localScale = Vector3.one;
        }
    }
}