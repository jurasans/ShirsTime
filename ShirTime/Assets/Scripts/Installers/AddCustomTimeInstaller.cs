namespace ShirTime.Installers
{
    using ShirTime.Infra;
    using ShirTime.UI;
    using UnityEngine;
    using Zenject;

    public class AddCustomTimeInstaller : MonoInstaller
    {
        [SerializeField]
        readonly TimeEntryView viewPrefab;
        public override void InstallBindings()
        {
            Container.Bind<AddDifferentTimeUI>().FromComponentInHierarchy().AsSingle();
            Container.Bind<EntryEditor>().AsSingle().NonLazy();
            //Container.BindMemoryPool<TimeEntryView, TimeEntryViewMemomoryPool>().ExpandByDoubling().FromComponentInNewPrefab(viewPrefab).AsSingle();

        }
    }
}
