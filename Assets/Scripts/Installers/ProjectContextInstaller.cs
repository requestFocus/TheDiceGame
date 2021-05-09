using Zenject;

public class ProjectContextInstaller : MonoInstaller<ProjectContextInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<ButtonHelper>().AsSingle();
        Container.Bind<GenericButton>().AsSingle();
    }
}