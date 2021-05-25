using UnityEngine;
using Zenject;

public class GameViewInstaller : MonoInstaller
{ 
#pragma warning disable
    [SerializeField] private DicePresenter _dicePresenterPrefab;
    [SerializeField] private CellPresenter _cellPresenterPrefab;
    [SerializeField] private UiManager _uiManager;
#pragma warning restore
    
    public override void InstallBindings()
    {
        Container.Bind<DicesManager>().AsSingle();
        Container.Bind<GameManager>().AsSingle();
        Container.Bind<DiceAmountChange>().AsSingle();
        Container.Bind<Cell>().AsTransient();
        Container.Bind<WindowsFactory>().AsSingle();
        
        Container.BindInterfacesAndSelfTo<CellsManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<Bank>().AsSingle();
        
        Container.Bind<UiManager>().FromComponentInNewPrefab(_uiManager).AsSingle();
        
        Container.BindFactory<DicePresenter, DicePresenter.Factory>().FromComponentInNewPrefab(_dicePresenterPrefab);
        Container.BindFactory<CellPresenter, CellPresenter.Factory>().FromComponentInNewPrefab(_cellPresenterPrefab);
    }
}