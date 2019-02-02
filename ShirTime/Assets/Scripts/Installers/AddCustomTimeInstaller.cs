namespace ShirTime.Installers
{
    using ShirTime.General;
    using ShirTime.Infra;
    using ShirTime.Settings;
    using ShirTime.UI;
    using UnityEngine;
    using Zenject;

    public class AddCustomTimeInstaller : MonoInstaller
    {
        [SerializeField]
        private TimeEntryView viewPrefab;
        [SerializeField]
        private UIDefaultSettings settings;
        public override void InstallBindings()
        {
            Container.Bind<AddDifferentTimeUI>().FromComponentInHierarchy().AsSingle();
            Container.Bind<EntryEditor>().AsSingle().NonLazy();
            Container.Bind<ViewPool>().AsSingle();
            Container.BindInstance(viewPrefab).AsSingle();
            Container.BindInstance(settings).AsSingle();

        }
    }
}
