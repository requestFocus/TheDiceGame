using Zenject;

public class SettingNewBalanceInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<Bank>().AsSingle();
        Container.Bind<GameManager>().AsSingle();
    }
}