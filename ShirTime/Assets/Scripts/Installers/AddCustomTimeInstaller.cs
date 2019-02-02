namespace ShirTime.Installers
{
    using ShirTime.Infra;
    using ShirTime.UI;
    using Zenject;

    public class AddCustomTimeInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<AddDifferentTimeUI>().FromComponentInHierarchy().AsSingle();
            Container.Bind<EntryEditor>().AsSingle().NonLazy();
            Container.BindMemoryPool<TimeEntryView, TimeEntryViewMemomoryPool>().ExpandByDoubling().AsSingle();

        }
    }
}
