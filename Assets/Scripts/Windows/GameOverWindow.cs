using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverWindow : GenericWindow
{
    protected override void OnButtonClose()
    {
        SceneManager.LoadScene("MenuScene");
    }
    
    public override void Setup()
    {
        transform.DOScale(Vector3.one, 0.3f).From(Vector3.zero);
    }
}