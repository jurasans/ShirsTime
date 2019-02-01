namespace ShirTime.Installers
{
    using Zenject;

    public class AddCustomTimeInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<AddDifferentTimeUI>().FromComponentInHierarchy().AsSingle();
        }
    }
}
