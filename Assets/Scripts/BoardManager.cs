using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class DiceManager : MonoBehaviour
{
    [SerializeField] private Transform _diceBoard;
    [SerializeField] private Transform _content;

    [Inject] private DicePresenter.Factory _dicePresenterFactory;

    void Start()
    {
        List<DicePresenter> _dicePresenters = new List<DicePresenter>();
        for (int i = 0; i < 3; i++)
        {
            DicePresenter dice = _dicePresenterFactory.Create();
            dice.transform.SetParent(_content);
            _dicePresenters.Add(dice);
        }

        _diceBoard.GetComponent<Image>().DOFade(1.0f, 3f).From(0);
    }
}
