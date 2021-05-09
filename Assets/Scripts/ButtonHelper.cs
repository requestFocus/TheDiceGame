using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHelper
{
    private Sequence _onButtonClickAnimation;
    private Sequence _onButtonLongPressAnimation;

    private readonly float _stepDuration = 0.3f;
    private readonly float _interval = 1f;

    public void OnButtonClick(GenericButton button, Action callback)
    {
        // if (_onButtonLongPressAnimation.IsActive())
        // {
        //     callback?.Invoke();
        //     _onButtonLongPressAnimation.Kill();
        //     return;
        // }

        button.interactable = false;

        Image[] images = button.GetComponentsInChildren<Image>();
        
        _onButtonClickAnimation = DOTween.Sequence();

        foreach (var image in images)
        {
            _onButtonClickAnimation.Insert(0,
                    image.transform.DORotate(Vector3.forward * -180f, 0.3f).SetEase(Ease.InSine))
                .Insert(0.3f, image.transform.DORotate(Vector3.forward * -360f, 0.3f).SetEase(Ease.OutSine));
        }

        _onButtonClickAnimation.Play().OnComplete(() =>
        {
            callback?.Invoke();
            button.interactable = true;
        }).OnKill(() => _onButtonClickAnimation = null);
    }

    public void OnButtonLongPress(GenericButton button, Action callback)
    {
        button.interactable = false;

        List<float> entryAlpha = new List<float>();
        Image[] images = button.GetComponentsInChildren<Image>();
        TextMeshProUGUI hint = button.GetComponentInChildren<TextMeshProUGUI>(true);

        _onButtonLongPressAnimation = DOTween.Sequence();
        
        foreach (var image in images)
        {
            entryAlpha.Add(image.color.a);
            _onButtonLongPressAnimation.Insert(0, image.DOFade(0f, _stepDuration).SetEase(Ease.InSine));
        }

        _onButtonLongPressAnimation.AppendCallback(() => hint.gameObject.SetActive(true))
            .AppendInterval(_interval)
            .AppendCallback(() => hint.gameObject.SetActive(false));

        for (var i = 0; i < images.Length; i++)
        {
            _onButtonLongPressAnimation.Insert(_stepDuration + _interval, images[i].DOFade(entryAlpha[i], _stepDuration).SetEase(Ease.OutSine));
        }

        _onButtonLongPressAnimation.Play().OnComplete(() =>
        {
            callback?.Invoke();
            button.interactable = true;
        }).OnKill(() => _onButtonLongPressAnimation = null);
    }
}