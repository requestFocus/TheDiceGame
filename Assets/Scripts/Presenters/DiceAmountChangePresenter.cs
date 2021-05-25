using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class DiceAmountChangePresenter : MonoBehaviour
{
    private GameConfig _gameConfig;
    private DiceAmountChange _model;
    private DicePresenter.Factory _dicePresenterFactory;
    private GameManager _gameManager;

    #pragma warning disable
    [SerializeField] private Image _addDiceImage;
    [SerializeField] private Button _addDiceButton;
    [SerializeField] private Image _removeDiceImage;
    [SerializeField] private Button _removeDiceButton;
    [SerializeField] private RectTransform _diceContainer;
    #pragma warning restore

    private readonly List<DicePresenter> _dicePresenters = new List<DicePresenter>();

    [Inject]
    private void Construct(GameConfig gameConfig, DiceAmountChange model, DicePresenter.Factory dicePresenterFactory,
        GameManager gameManager)
    {
        _gameConfig = gameConfig;
        _model = model;
        _dicePresenterFactory = dicePresenterFactory;
        _gameManager = gameManager;
    }
    
    private void Start()
    {
        _addDiceButton.onClick.AddListener(AddDice);
        _removeDiceButton.onClick.AddListener(RemoveDice);

        _gameManager.DiceAmountChanged += UpdateDiceAmountChangePresenters;

        for (int i = 0; i < _gameConfig.AmountOfDices; i++)
        {
            CreateDicePresenter();
        }
        
        ValidateView();
    }

    private void AddDice()
    {
        if (_gameConfig.AmountOfDices < _gameConfig.MaxAmountOfDices)
        {
            _model.AddDice();
            CreateDicePresenter();
            UpdateDiceAmountChangePresenters();

            ValidateView();
        }
    }

    private void RemoveDice()
    {
        if (_gameConfig.AmountOfDices > _gameConfig.MinAmountOfDices)
        {
            _model.RemoveDice();
            DestroyDicePresenter();
            UpdateDiceAmountChangePresenters();
            
            ValidateView();
        }
    }

    private void ValidateView()
    {
        Color faded = new Color(_addDiceImage.color.r, _addDiceImage.color.g, _addDiceImage.color.b, 0.3f);
        Color full = new Color(_addDiceImage.color.r, _addDiceImage.color.g, _addDiceImage.color.b, 1f);
        
        _addDiceImage.color = _dicePresenters.Count != _gameConfig.MaxAmountOfDices ? full : faded;
        _removeDiceImage.color = _dicePresenters.Count != _gameConfig.MinAmountOfDices ? full : faded;
    }

    private void CreateDicePresenter()
    {
        DicePresenter dice = _dicePresenterFactory.Create();
        Transform diceTransform = dice.transform;
        diceTransform.Rotate(new Vector3(0, 0, 45));
        diceTransform.SetParent(_diceContainer, false);
        diceTransform.transform.DOScale(Vector3.one * 0.3f, 0.2f).SetEase(Ease.InBounce).From(Vector3.zero);
        _dicePresenters.Add(dice);
    }

    private void DestroyDicePresenter()
    {
        Transform diceTransform = _diceContainer.GetChild(_dicePresenters.Count - 1);
        diceTransform.DOScale(Vector3.zero, 0.1f)
            .OnComplete(() => { Destroy(diceTransform.gameObject); });
        _dicePresenters.RemoveAt(_dicePresenters.Count - 1);
    }

    private void UpdateDiceAmountChangePresenters()
    {
        var layoutGroup = _diceContainer.GetComponent<HorizontalLayoutGroup>();
        switch (_gameConfig.AmountOfDices)
        {
            case 3:
                layoutGroup.spacing = -40;
                break;
            case 2:
                layoutGroup.spacing = -90;
                break;
            default:
                layoutGroup.spacing = 0;
                break;
        }
    }

    private void OnDestroy()
    {
        _addDiceButton.onClick.RemoveListener(AddDice);
        _removeDiceButton.onClick.RemoveListener(RemoveDice);
        
        _gameManager.DiceAmountChanged -= UpdateDiceAmountChangePresenters;
    }
}