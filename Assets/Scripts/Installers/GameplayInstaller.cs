using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public class GameplayInstaller : MonoInstaller
{ 
#pragma warning disable
    [SerializeField] private DicePresenter _dicePresenterPrefab;
    [FormerlySerializedAs("_cellPresenterPrefab")] [SerializeField] private BettingCellPresenter _bettingCellPresenterPrefab;
    [SerializeField] private UiManager _uiManager;
#pragma warning restore
    
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<GameplayPanel>().AsSingle();
        Container.Bind<DicesService>().AsSingle();
        Container.Bind<UiManager>().FromComponentInNewPrefab(_uiManager).AsSingle();
        
        Container.Bind<DiceAmountChange>().AsSingle();
        Container.Bind<BettingCell>().AsTransient();
        Container.Bind<Dice>().AsSingle();
        Container.BindInterfacesAndSelfTo<Bank>().AsSingle();
        Container.BindInterfacesAndSelfTo<BettingSystem>().AsSingle();
        Container.BindInterfacesAndSelfTo<WinScoreIndicator>().AsSingle();
        
        Container.Bind<WindowsFactory>().AsSingle();
        
        Container.BindFactory<DicePresenter, DicePresenter.Factory>().FromComponentInNewPrefab(_dicePresenterPrefab);
        Container.BindFactory<int, BettingCellPresenter, BettingCellPresenter.Factory>().FromComponentInNewPrefab(_bettingCellPresenterPrefab);
    }
}