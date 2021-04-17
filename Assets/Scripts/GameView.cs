using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameView :MonoBehaviour
{
    [SerializeField] private Button _goBackButton;

    private void Start()
    {
        _goBackButton.onClick.AddListener(GoBack);
    }

    private void GoBack()
    {
        SceneManager.LoadScene("Scenes/Menu");
    }
}