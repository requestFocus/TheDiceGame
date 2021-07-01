using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UiManager : MonoBehaviour
{
#pragma warning disable CS0649
    [SerializeField] private Image _overlay;
#pragma warning restore CS0649
    
    private WindowsFactory _windowsFactory;
    
    [Inject]
    private void Construct(WindowsFactory windowsFactory)
    {
        _windowsFactory = windowsFactory;
    }

    private void Start()
    {
        Dictionary<string, GenericWindow> prefabsDictionary = new Dictionary<string, GenericWindow>();
        GenericWindow[] windows = Resources.LoadAll<GenericWindow>("Prefabs");

        foreach (var prefab in windows)
        {
            prefabsDictionary.Add(prefab.name, prefab);
        }
        
        _windowsFactory.BindWindows(prefabsDictionary);
    }

    public void HideOverlay()   // POZBĄDZ sie na korzyść async/await na otwartym oknie?
    {
        _overlay.gameObject.SetActive(false);
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