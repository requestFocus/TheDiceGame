using UnityEngine;
using Zenject;

public class MenuInstaller : MonoInstaller
{ 
#pragma warning disable
    [SerializeField] private UiManager _uiManager;
#pragma warning restore
    
    public override void InstallBindings()
    {
        Container.Bind<WindowsFactory>().AsSingle();
        
        Container.Bind<UiManager>().FromComponentInNewPrefab(_uiManager).AsSingle();
    }
}