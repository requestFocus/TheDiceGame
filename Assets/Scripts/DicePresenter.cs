using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class DicePresenter : MonoBehaviour
{
    [SerializeField] private Text _score;
    
    void Start()
    {
        _score.text = Random.Range(1, 6).ToString();
    }

    public class Factory : PlaceholderFactory<DicePresenter>
    {
    }
}
