using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class DicePresenter : MonoBehaviour
{
#pragma warning disable
    [SerializeField] private Sprite[] _dots;
    [SerializeField] private Image _diceImage;
#pragma warning restore
    
    private int _score;
    
    private void Awake()
    {
        _score = Random.Range(0, 6);
        _diceImage.sprite = _dots[_score];
    }

    public float[] GetDimensions()
    {
        return new[] {_diceImage.GetComponent<RectTransform>().rect.width, _diceImage.GetComponent<RectTransform>().rect.height};
    }

    public int GetScore()
    {
        return _score + 1;
    }

    public Image GetDiceImage()
    {
        return _diceImage;
    }

    public class Factory : PlaceholderFactory<DicePresenter>
    {
    }
}
