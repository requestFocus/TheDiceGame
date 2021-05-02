using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class DiceAmountChangePresenter : MonoBehaviour
{
    [Inject] private GameConfig _gameConfig;
    [Inject] private DiceAmountChange _diceAmountChange;
    
    [SerializeField] private Button _addDiceButton;
    [SerializeField] private Button _removeDiceButton;

    private void Start()
    {
        _addDiceButton.onClick.AddListener(AddDice);
        _removeDiceButton.onClick.AddListener(RemoveDice);
    }

    private void AddDice()
    {
        if (_gameConfig.AmountOfDices < _gameConfig.MaxAmountOfDices)
        {
            _diceAmountChange.AddDice();
        }
    }

    private void RemoveDice()
    {
        if (_gameConfig.AmountOfDices > _gameConfig.MinAmountOfDices)
        {
            _diceAmountChange.RemoveDice();
        }
    }

    private void OnDestroy()
    {
        _addDiceButton.onClick.RemoveListener(AddDice);
        _removeDiceButton.onClick.RemoveListener(RemoveDice);
    }
}