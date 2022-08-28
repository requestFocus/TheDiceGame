using System.Collections.Generic;
using Zenject;

public class WindowsFactory
{
    private Dictionary<string, GenericWindow> _windowsPrefabs;

    private DiContainer _container;
    
    [Inject]
    private void Construct(DiContainer container)
    {
        _container = container;
    }

    public void BindWindows(Dictionary<string, GenericWindow> windowPrefabs)
    {
        _windowsPrefabs = windowPrefabs;
    }
    
    public T CreateWindow<T>() where T : GenericWindow
    {
        string reference = typeof(T).ToString();
        
        return _container.InstantiatePrefabForComponent<T>(_windowsPrefabs[reference]);
    } 
}