using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class CellPresenter2 : MonoBehaviour
{
#pragma warning disable CS0649
    [SerializeField] private Image _image;
    [SerializeField] private SelectableButton _selectableButton;
#pragma warning restore CS0649

    private Cell2 _model;

    [Inject]
    private void Construct(Cell2 model)
    {
        _model = model;
    }
    
    public SelectableButton SelectableButton => _selectableButton;
    
    public void ChangeState(Color color)
    {
        if (_model.GetSelectedState())
        {
            DeselectCell();
            _image.color = Color.white;
        }
        else
        {
            SelectCell();
            _image.color = color;
        }
    }

    private void SelectCell()
    {
        _model.SelectCell();
    }

    private void DeselectCell()
    {
        _model.DeselectCell();
    }

    public bool GetSelectedState()
    {
        return _model.GetSelectedState();
    }

    public void SetCellId(int value)
    {
        _model.SetCellId(value);
    }
    
    public class Factory : PlaceholderFactory<CellPresenter2>
    {
    }
}