using UnityEngine;
using Zenject;

public class TestSceneInstaller : MonoInstaller
{
#pragma warning disable
    [SerializeField] private CellPresenter2 _cellPresenterPrefab;
#pragma warning restore
    
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<CellsManager2>().AsSingle();
        Container.Bind<BettingSystem2>().AsSingle();
        Container.Bind<Cell2>().AsTransient();

        Container.BindFactory<CellPresenter2, CellPresenter2.Factory>().FromComponentInNewPrefab(_cellPresenterPrefab);
    }
}