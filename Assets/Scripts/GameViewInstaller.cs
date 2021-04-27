using UnityEngine;
using Zenject;

public class GameViewInstaller : MonoInstaller
{
    [SerializeField] private DicePresenter _dicePresenterPrefab;

    public override void InstallBindings()
    {
        Container.Bind<DicesManager>().AsSingle();
        Container.BindFactory<DicePresenter, DicePresenter.Factory>().FromComponentInNewPrefab(_dicePresenterPrefab);
    }
}