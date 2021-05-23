using System;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class HowToPlayWindow : GenericWindow
{
    [Inject]
    private void Construct()
    {
    }
    
    protected override void Start()
    {
        base.Start();
    }

    protected override void OnButtonClose(Action callback)
    {
        Destroy(gameObject);
    }

    public override void Setup()
    {
        transform.DOScale(Vector3.one, 0.3f).From(Vector3.zero);
    }
}