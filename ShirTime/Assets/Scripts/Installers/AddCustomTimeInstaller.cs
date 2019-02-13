namespace ShirTime.Installers
{
    using FantomLib;
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
            Container.Bind<CustomTimeEditorUI>().FromComponentInHierarchy().AsSingle();
			Container.Bind<TimePickerController>().FromComponentInHierarchy().AsSingle();
            Container.Bind<DatePickerController>().FromComponentInHierarchy().AsSingle();

            Container.Bind<EntryEditor>().AsSingle().NonLazy();
            Container.Bind<ViewPool>().AsSingle();
            Container.BindInstance(viewPrefab).AsSingle();
            Container.BindInstance(settings).AsSingle();

        }
    }
}
