using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class SettingNewBankBalancePresenter : MonoBehaviour
{
    [SerializeField] private GenericButton _playButton;
    [SerializeField] private GenericButton _goBackButton;
    [SerializeField] private TextMeshProUGUI _newBankBalance;

    private ButtonHelper _buttonHelper;
    private int _newBankBalanceAmount;
    private Sequence _drawingBalanceAnimation;

    [Inject]
    private void Construct(ButtonHelper buttonHelper)
    {
        _buttonHelper = buttonHelper;
    }

    private void Start()
    {
        _playButton.onClick.AddListener(() => _buttonHelper.OnButtonClick(_playButton, OnPlayClick));
        _playButton.onLongPress.AddListener(() => _buttonHelper.OnButtonLongPress(_playButton, () => { }));
        
        _goBackButton.onClick.AddListener(() => _buttonHelper.OnButtonClick(_goBackButton, OnQuitClick));
        _goBackButton.onLongPress.AddListener(() => _buttonHelper.OnButtonLongPress(_goBackButton, () => { }));

        
        SetUpNewBankBalance();
    }

    private void SetUpNewBankBalance()
    {
        _playButton.interactable = false;
        
        AnimateNewBankBalanceDrawing().Play()
            .OnComplete(() =>
            {
                PlayerPrefs.SetInt("Balance", _newBankBalanceAmount);
                _playButton.interactable = true;    
            });
    }

    private Sequence AnimateNewBankBalanceDrawing()
    {
        _drawingBalanceAnimation = DOTween.Sequence();
        
        for (int i = 0; i < 30; i++)
        {
            _drawingBalanceAnimation.AppendCallback(() =>
            {
                _newBankBalanceAmount = Random.Range(200, 800);
                _newBankBalance.text = _newBankBalanceAmount.ToString();
            });
            _drawingBalanceAnimation.AppendInterval(0.05f);
        }

        return _drawingBalanceAnimation;
    }

    private void OnQuitClick()
    {
        SceneManager.LoadScene("Scenes/MenuScene");
    }

    private void OnPlayClick()
    {
        SceneManager.LoadScene("Scenes/GameplayScene");
    }

    private void OnDestroy()
    {
        _playButton.onClick.RemoveAllListeners();
        _playButton.onLongPress.RemoveAllListeners();
        
        _goBackButton.onClick.RemoveAllListeners();
        _goBackButton.onLongPress.RemoveAllListeners();
    }
}