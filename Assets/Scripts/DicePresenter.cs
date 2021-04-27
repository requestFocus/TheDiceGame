using TMPro;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class DicePresenter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private Sprite[] _dots;
    [SerializeField] private Image _diceImage;

    private int _score;
    
    private void Awake()
    {
        _score = Random.Range(0, 6);
        _scoreText.text = _score.ToString();
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

    public class Factory : PlaceholderFactory<DicePresenter>
    {
    }
}
