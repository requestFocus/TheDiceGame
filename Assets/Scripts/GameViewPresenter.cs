using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class GameViewPresenter : MonoBehaviour
{
    [SerializeField] private Button _goBackButton;
    [SerializeField] private Button _tossDicesButton;
    [SerializeField] private RectTransform _contentTransform;
    [SerializeField] private TextMeshProUGUI _totalScoreText;

    private DicesManager _dicesManager;
    private List<DicePresenter> _dicePresenters;

    [Inject]
    private void Construct(DicesManager dicesManager)
    {
        _dicesManager = dicesManager;
    }

    private void Start()
    {
        _goBackButton.onClick.AddListener(GoBack);
        _tossDicesButton.onClick.AddListener(TossDices);

        TossDices();
    }

    private void OnDestroy()
    {
        _goBackButton.onClick.RemoveListener(GoBack);
        _tossDicesButton.onClick.RemoveListener(TossDices);
    }
    
    private void GoBack()
    {
        SceneManager.LoadScene("Scenes/Menu");
    }

    private void TossDices()
    {
        DeleteDices();
        _dicesManager.ClearOccupiedPositions();

        _dicePresenters = _dicesManager.CreateDices();
        DistributeDices();

        CalculateScore();
    }

    private void CalculateScore()
    {
        int totalScore = 0;
        foreach (var dice in _dicePresenters)
        {
            totalScore += dice.GetScore();
        }

        _totalScoreText.text = totalScore.ToString();
    }

    private void DistributeDices()
    {
        Rect rect = _contentTransform.rect;
        foreach (var dicePresenter in _dicePresenters)
        {
            dicePresenter.transform.SetParent(_contentTransform);
            dicePresenter.transform.localPosition 
                = _dicesManager.GetUniqueRandomPosition(rect.width, rect.height, dicePresenter.GetDimensions());
            dicePresenter.transform.Rotate(0, 0, Random.Range(0, 360));
        }
    }

    private void DeleteDices()
    {
        for (int i = 0; i < _contentTransform.childCount; i++)
        {
            Destroy(_contentTransform.GetChild(i).gameObject);
        }
    }
}