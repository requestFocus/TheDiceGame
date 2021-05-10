using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GenericWindow : MonoBehaviour
{
    [SerializeField] private Image _overlay;
    [SerializeField] private Button _closeButton;
    [SerializeField] private TextMeshProUGUI _text;

    private void Start()
    {
        _text.text = "generic window";
        transform.localScale = Vector3.one;
        
        _closeButton.onClick.AddListener(() => Destroy(gameObject));
    }

    public class Factory : PlaceholderFactory<GenericWindow> {}
}
