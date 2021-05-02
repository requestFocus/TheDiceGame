using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuViewPresenter : MonoBehaviour
{
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _howToPlayButton;
    [SerializeField] private Button _quitButton;
    [SerializeField] private GameObject _instructions;

    private bool _instructionsState;

    private void Start()
    {
        _playButton.onClick.AddListener(OnPlayClick);
        _howToPlayButton.onClick.AddListener(OnHowToPlayClick);
        _quitButton.onClick.AddListener(OnQuitClick);
        
        _instructions.SetActive(_instructionsState);
    }

    private void OnQuitClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
    }

    private void OnHowToPlayClick()
    {
        _instructionsState = !_instructionsState;
        _instructions.SetActive(_instructionsState);
    }

    private void OnPlayClick()
    {
        SceneManager.LoadScene("Scenes/Gameplay");
    }
}