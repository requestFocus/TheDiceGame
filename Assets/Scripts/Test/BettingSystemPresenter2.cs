using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class BettingSystemPresenter2 : MonoBehaviour
{
    [SerializeField] private Button _testButton;
    [SerializeField] private GridLayoutGroup _layoutGroup;
    [SerializeField] private Transform _contentTransform;
    [SerializeField] private Button[] _colorButtons;
    
    private CellsManager2 _cellsManager;
    private BettingSystem2 _model;

    [Inject]
    private void Construct(CellsManager2 cellsManager, BettingSystem2 model)
    {
        _cellsManager = cellsManager;
        _model = model;
    }

    private void Awake()
    {
        CreateCellPresenters();
        AssignCellsToParentTransform();
    }

    private void Start()
    {
        _testButton.onClick.AddListener(() => SceneManager.LoadScene("MenuScene"));

        foreach (var button in _colorButtons)
        {
            Color color = button.GetComponentInChildren<Image>().color;
            UnityAction colorSelectedToSubscribe = () =>
            {
                _cellsManager.SetCellColor(color);
            };
            button.onClick.AddListener(colorSelectedToSubscribe);
        }
        
        _cellsManager.SetCellColor(_colorButtons[0].GetComponentInChildren<Image>().color);
    }

    private void CreateCellPresenters()
    {
        var amountOfCells = _model.GetMaxAmountOfCellsVertically() * _model.GetMaxAmountOfCellsHorizontally();
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
            cellTransform.SetParent(_contentTransform);
            cellTransform.localScale = Vector3.one;
        }

        _layoutGroup.constraintCount = _model.GetMaxAmountOfCellsHorizontally();
    }

    private void OnDestroy()
    {
        _testButton.onClick.RemoveListener(() => SceneManager.LoadScene("MenuScene"));

        foreach (var button in _colorButtons)
        {
            button.onClick.RemoveAllListeners();
        }
    }
}