using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UiManager : MonoBehaviour
{
#pragma warning disable CS0649
    [SerializeField] private Image _overlay;
    [SerializeField] private TextMeshProUGUI _inputBlockade;
    [SerializeField] private List<GenericWindow> _windowPrefabs;
#pragma warning restore CS0649
    
    private WindowsFactory _windowsFactory;
    
    [Inject]
    private void Construct(WindowsFactory windowsFactory)
    {
        _windowsFactory = windowsFactory;
    }

    private void Start()
    {
        // TODO to nie może tak zostać, gdzieś to musi zostać wsadzone, żeby dało się to WCZYTAC
        Dictionary<string, GenericWindow> prefabsDictionary = new Dictionary<string, GenericWindow>()
        {
            {"HowToPlayWindow", _windowPrefabs[0]},
            {"GameOverWindow", _windowPrefabs[1]},
        };
        
        _windowsFactory.BindWindows(prefabsDictionary);
    }

    public void HideOverlay()   // POZBĄDZ sie na korzyść async/await na otwartym oknie?
    {
        _overlay.gameObject.SetActive(false);
    }

    public void DisableInputBlockade()
    {
        _inputBlockade.gameObject.SetActive(false);
    }

    public void EnableInputBlockade()
    {
        _inputBlockade.gameObject.SetActive(true);
    }
    
    public T ShowWindow<T>() where T : GenericWindow
    {
        var window = _windowsFactory.CreateWindow<T>();
        window.transform.SetParent(transform, false);
        window.Setup();
        
        _overlay.gameObject.SetActive(window.WithOverlay());
        
        return window;
    }
}