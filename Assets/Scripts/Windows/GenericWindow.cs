using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public abstract class GenericWindow : MonoBehaviour
{
#pragma warning disable CS0649
    [SerializeField] private GenericButton _closeButton;
    [SerializeField] protected bool _withOverlay;
#pragma warning restore CS0649

    private UiManager _uiManager;
    private ButtonHelper _buttonHelper;
    
    [Inject]
    private void Construct(UiManager uiManager, ButtonHelper buttonHelper)
    {
        _uiManager = uiManager;
        _buttonHelper = buttonHelper;
    }
    
    protected virtual void Start()
    {
        transform.localScale = Vector3.zero;
        
        // _closeButton.onClick.AddListener(OnButtonClose);
        
        _closeButton.onClick.AddListener(() => _buttonHelper.OnButtonClick(_closeButton, OnButtonClose));
        _closeButton.onLongPress.AddListener(() => _buttonHelper.OnButtonLongPress(_closeButton, () => { }));
    }

    public abstract void Setup();

    public bool WithOverlay()
    {
        return _withOverlay;
    }

    protected virtual void OnButtonClose()
    {
    }

    protected virtual void OnDestroy()
    {
        _closeButton.onClick.RemoveAllListeners();
        _uiManager.HideOverlay();
    }
}
