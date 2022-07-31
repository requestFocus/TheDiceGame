using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class DicePresenter : MonoBehaviour
{
#pragma warning disable
    [SerializeField] private Sprite[] _dots;
    [SerializeField] private Image _diceImage;

    [SerializeField] private Image[] _animatedSides;
    [SerializeField] private Transform _animatedSidesTransform;
#pragma warning restore

    private Dice _model;
    
    [Inject]
    private void Construct(Dice model)
    {
        _model = model;
    }
    
    private int _drawnDiceId;

    private const float _oneTurn = 0.2f;

    private int _previousRandomId2 = 0;

    private void Awake()
    {
        _drawnDiceId = _model.GetRandomDotsAmount();
        _diceImage.sprite = GetDotsSprite(_drawnDiceId);
    }

    public float[] GetDimensions()
    {
        return new[]
            {_diceImage.GetComponent<RectTransform>().rect.width, _diceImage.GetComponent<RectTransform>().rect.height};
    }

    public int GetDiceValue()
    {
        return _drawnDiceId + 1;
    }

    public int GetGeneratedRandomId()
    {
        return _drawnDiceId;
    }

    private Sprite GetDotsSprite(int id)
    {
        return _dots[id];
    }

    public Image GetDiceImage()
    {
        return _diceImage;
    }

    public Transform GetAnimatedSidesContainerTransform()
    {
        return _animatedSidesTransform;
    }

    public void AnimateDiceMovement(int dotsAmount)
    {
        Sequence sequence = DOTween.Sequence()
            .PrependCallback(() =>
            {
                _animatedSides[0].gameObject.SetActive(true);
                _animatedSides[1].gameObject.SetActive(true);
            });
    
        for (int i = 0; i < 5; i++)
        {
            int iterationNumber = i;
            int randomId1 = _model.GetRandomDotsAmount();
            int randomId2 = _model.GetRandomDotsAmount();
    
            sequence
                .InsertCallback(_oneTurn * i, () =>
                {
                    _animatedSides[0].rectTransform.sizeDelta = new Vector2(250, 250);
                    _animatedSides[1].rectTransform.sizeDelta = new Vector2(0, 250);
    
                    if (iterationNumber == 4)
                    {
                        _animatedSides[0].sprite = GetDotsSprite(_previousRandomId2);
                        _animatedSides[1].sprite = GetDotsSprite(dotsAmount);
                    }
                    else if (iterationNumber > 0)
                    {
                        _animatedSides[0].sprite = GetDotsSprite(_previousRandomId2);
                        _animatedSides[1].sprite = GetDotsSprite(randomId2);
                    }
                    else
                    {
                        _animatedSides[0].sprite = GetDotsSprite(randomId1);
                        _animatedSides[1].sprite = GetDotsSprite(randomId2);
                    }
                })
                .InsertCallback(_oneTurn * (i + 1f),
                    () =>
                    {
                        AnimateWidth(_animatedSides[0], 250, 0);
                        AnimateWidth(_animatedSides[1], 0, 250);
                        iterationNumber += 1;
                        _previousRandomId2 = randomId2;
                    })
                ;
        }
    
        sequence.Play().OnComplete(() =>
        {
            _animatedSides[0].gameObject.SetActive(false);
            _animatedSides[1].gameObject.SetActive(false);
        });
    }
    
    private void AnimateWidth(Image image, float startValue, float endValue)
    {
        DOTween.To(
            () => startValue,
            x => { image.rectTransform.sizeDelta = new Vector2(x, image.rectTransform.rect.height); },
            endValue,
            0.15f).Play();  
    }
    
    public class Factory : PlaceholderFactory<DicePresenter>
    {
    }
}