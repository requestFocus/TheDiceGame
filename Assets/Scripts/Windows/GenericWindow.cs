using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public abstract class GenericWindow : MonoBehaviour
{
#pragma warning disable CS0649
    [SerializeField] private Button _closeButton;
    [SerializeField] protected bool _withOverlay;
#pragma warning restore CS0649

    private UiManager _uiManager;
    
    [Inject]
    private void Construct(UiManager uiManager)
    {
        _uiManager = uiManager;
    }
    
    protected virtual void Start()
    {
        transform.localScale = Vector3.zero;
        
        _closeButton.onClick.AddListener(() => OnButtonClose(() => { }));
    }

    public abstract void Setup();

    public bool WithOverlay()
    {
        return _withOverlay;
    }

    protected virtual void OnButtonClose(Action callback)
    {
        callback?.Invoke();
    }

    protected virtual void OnDestroy()
    {
        _closeButton.onClick.RemoveAllListeners();
        _uiManager.HideOverlay();
    }
}
