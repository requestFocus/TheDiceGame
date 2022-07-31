using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public class NewBalanceGeneratorInstaller : MonoInstaller
{
#pragma warning disable
    [FormerlySerializedAs("_cellPresenterPrefab")] [SerializeField] private BettingCellPresenter _bettingCellPresenterPrefab;
    [SerializeField] private DicePresenter _dicePresenterPrefab;
#pragma warning restore
    
    public override void InstallBindings()
    {
        Container.Bind<Bank>().AsSingle();
        Container.Bind<GameplayManager>().AsSingle();
        Container.Bind<DicesManager>().AsSingle();
        Container.Bind<NewBankBalanceGenerator>().AsSingle();
        // Container.BindInterfacesAndSelfTo<BettingCellsManager>().AsSingle();
        
        Container.BindFactory<int, BettingCellPresenter, BettingCellPresenter.Factory>().FromComponentInNewPrefab(_bettingCellPresenterPrefab);
        Container.BindFactory<DicePresenter, DicePresenter.Factory>().FromComponentInNewPrefab(_dicePresenterPrefab);

    }
}