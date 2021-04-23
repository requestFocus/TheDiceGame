using UnityEngine;
using Zenject;

public class GameViewInstaller : MonoInstaller
{
    [SerializeField] private DicePresenter _dicePresenterPrefab;

    public override void InstallBindings()
    {
        Container.BindFactory<DicePresenter, DicePresenter.Factory>().FromComponentInNewPrefab(_dicePresenterPrefab);
    }
}