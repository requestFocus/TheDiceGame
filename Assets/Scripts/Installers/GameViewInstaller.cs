using UnityEngine;
using Zenject;

public class GameViewInstaller : MonoInstaller
{ 
#pragma warning disable
    [SerializeField] private DicePresenter _dicePresenterPrefab;
    [SerializeField] private UiManager _uiManager;
#pragma warning restore
    
    public override void InstallBindings()
    {
        Container.Bind<DicesManager>().AsSingle();
        Container.Bind<GameManager>().AsSingle();
        Container.Bind<DiceAmountChange>().AsSingle();
        Container.Bind<WindowsFactory>().AsSingle();
        Container.Bind<BettingSlider>().AsCached();
        Container.BindInterfacesAndSelfTo<Bank>().AsSingle();
        
        Container.Bind<UiManager>().FromComponentInNewPrefab(_uiManager).AsSingle();
        
        Container.BindFactory<DicePresenter, DicePresenter.Factory>().FromComponentInNewPrefab(_dicePresenterPrefab);
    }
}