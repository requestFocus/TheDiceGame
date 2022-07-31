using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class BettingCellPresenter : MonoBehaviour
{
#pragma warning disable CS0649
    [SerializeField] private Image _image;
    [SerializeField] private Image _winningIndicator;
    [SerializeField] private SelectableButton _selectableButton;
    [SerializeField] private TextMeshProUGUI _cellIdText;

#pragma warning restore CS0649

    public SelectableButton SelectableButton => _selectableButton;
    public Image WinningIndicator => _winningIndicator;
    public TextMeshProUGUI CellIdText => _cellIdText;
    
    private BettingCell _model;

    [Inject]
    private void Construct(int index, BettingCell model)
    {
        _model = model;
        _model.SetCellId(index);
    }

    private void Start()
    {
        SetCellIdText();
    }
    
    public void ChangeState()
    {
        if (!_model.IsSelected())
        {
            SelectCell();
            _image.color = Color.red;
        }
        else
        {
            DeselectCell();
            _image.color = Color.white;
        }
    }

    private void SelectCell()
    {
        _model.SelectCell();
    }

    public void DeselectCell()
    {
        _model.DeselectCell();
    }

    public bool IsSelected()
    {
        return _model.IsSelected();
    }

    public void SetAsEnabled()
    {
        _image.color = Color.white;
        _cellIdText.color = Color.black;
    }

    public void EnableInteractability()
    {
        _selectableButton.interactable = true;
    }

    public void SetAsDisabled()
    {
        _image.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        _cellIdText.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
    }

    public void DisableInteractability()
    {
        _selectableButton.interactable = false;
    }

    public int GetCellId()
    {
        return _model.GetCellId();
    }

    public void SetCellId(int value)
    {
        _model.SetCellId(value);
    }
    
    public void SetCellIdText()
    {
        _cellIdText.text = (_model.GetCellId() + 1).ToString();
    }

    public class Factory : PlaceholderFactory<int, BettingCellPresenter>
    {
    }
}