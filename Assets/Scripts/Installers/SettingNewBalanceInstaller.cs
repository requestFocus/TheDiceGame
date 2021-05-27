using UnityEngine;
using Zenject;

public class SettingNewBalanceInstaller : MonoInstaller
{
#pragma warning disable
    [SerializeField] private CellPresenter _cellPresenterPrefab;
    [SerializeField] private DicePresenter _dicePresenterPrefab;
#pragma warning restore
    
    public override void InstallBindings()
    {
        Container.Bind<Bank>().AsSingle();
        Container.Bind<GameManager>().AsSingle();
        Container.Bind<DicesManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<CellsManager>().AsSingle();
        
        Container.BindFactory<CellPresenter, CellPresenter.Factory>().FromComponentInNewPrefab(_cellPresenterPrefab);
        Container.BindFactory<DicePresenter, DicePresenter.Factory>().FromComponentInNewPrefab(_dicePresenterPrefab);

    }
}