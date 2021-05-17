using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class HowToPlayWindow : GenericWindow
{
    private UiManager _uiManager;
    
    [Inject]
    private void Construct(UiManager uiManager)
    {
        _uiManager = uiManager;
    }
    
    protected override void Start()
    {
        base.Start();
        
        ProveExistence();
    }

    protected override void OnButtonClose(Action callback)
    {
        _uiManager.HideOverlay();
        Destroy(gameObject);
    }

    public override void Setup()
    {
        transform.DOScale(Vector3.one, 0.3f).From(Vector3.zero);
    }

    private void ProveExistence()
    {
        Debug.Log("how to play window, boom");
    }
}