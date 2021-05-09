using UnityEngine;
using Zenject;

public class GameViewInstaller : MonoInstaller
{
    [SerializeField] private DicePresenter _dicePresenterPrefab;
    [SerializeField] private GenericWindow _genericWindowPrefab;

    public override void InstallBindings()
    {
        Container.Bind<DicesManager>().AsSingle();
        Container.Bind<GameManager>().AsSingle();
        Container.Bind<DiceAmountChange>().AsSingle();
        Container.BindInterfacesAndSelfTo<Bank>().AsSingle();
        
        Container.BindFactory<DicePresenter, DicePresenter.Factory>().FromComponentInNewPrefab(_dicePresenterPrefab);
        Container.BindFactory<GenericWindow, GenericWindow.Factory>().FromComponentInNewPrefab(_genericWindowPrefab);
    }
}