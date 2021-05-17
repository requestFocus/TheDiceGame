using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverWindow : GenericWindow
{
    protected override void Start()
    {
        base.Start();
        
        ProveExistence();
    }

    protected override void OnButtonClose(Action callback)
    {
        SceneManager.LoadScene("MenuScene");
    }
    
    public override void Setup()
    {
        transform.DOScale(Vector3.one, 0.3f).From(Vector3.zero);
    }

    private void ProveExistence()
    {
        Debug.Log("game over window, boom");
    }
}