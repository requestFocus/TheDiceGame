using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class MenuViewPresenter : MonoBehaviour
{
    [SerializeField] private Image _blink;
    [SerializeField] private GenericButton _playButton;
    [SerializeField] private GenericButton _setUpNewBalanceButton;
    [SerializeField] private GenericButton _quitButton;

    private ButtonHelper _buttonHelper;
    private Sequence _blinkAnimation;

    [Inject]
    private void Construct(ButtonHelper buttonHelper)
    {
        _buttonHelper = buttonHelper;
    }
    
    private void Start()
    {
        _playButton.onClick.AddListener(() => _buttonHelper.OnButtonClick(_playButton, OnPlayClick));
        _playButton.onLongPress.AddListener(() => _buttonHelper.OnButtonLongPress(_playButton, () => { }));
        
        _quitButton.onClick.AddListener(() => _buttonHelper.OnButtonClick(_quitButton, OnQuitClick));
        _quitButton.onLongPress.AddListener(() => _buttonHelper.OnButtonLongPress(_quitButton, () => { }));
        
        _setUpNewBalanceButton.onClick.AddListener(() => _buttonHelper.OnButtonClick(_setUpNewBalanceButton, OnSetUpNewBankClick));
        _setUpNewBalanceButton.onLongPress.AddListener(() => _buttonHelper.OnButtonLongPress(_setUpNewBalanceButton, () => { }));

        _playButton.gameObject.SetActive(PlayerPrefs.GetInt("Balance") != 0);
        AnimateBlink();
    }

    private void OnQuitClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
    }

    private void OnPlayClick()
    {
        SceneManager.LoadScene("Scenes/GameplayScene");
    }

    private void OnSetUpNewBankClick()
    {
        SceneManager.LoadScene("Scenes/SettingNewBankScene");
    }

    private void AnimateBlink()
    {
        _blinkAnimation = DOTween.Sequence();
        _blinkAnimation
            .Append(_blink.transform.DOMoveX(Screen.width, 1f).From(0))
            .AppendInterval(3.0f)
            .Play().SetLoops(-1);
    }
    
    private void OnDestroy()
    {
        _playButton.onLongPress.RemoveAllListeners();
        _playButton.onClick.RemoveAllListeners();
        
        _quitButton.onClick.RemoveAllListeners();
        _quitButton.onLongPress.RemoveAllListeners();
        
        _setUpNewBalanceButton.onClick.RemoveAllListeners();
        _setUpNewBalanceButton.onLongPress.RemoveAllListeners();

        if (_blinkAnimation.IsPlaying())
        {
            _blinkAnimation.Kill();
        }
    }
}