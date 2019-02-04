namespace ShirTime.Installers
{
    using ShirTime.Infra;
    using ShirTime.UI;
    using UnityEngine;
    using Zenject;

    public class ApplicationInstaller : MonoInstaller
    {
        [SerializeField]
        private string databaseName;
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<TimeKeepingManager>().AsSingle().NonLazy();
            Container.Bind<IMainUICallbacks>().FromComponentInHierarchy().AsSingle();
            Container.Bind<ICustomTimeUI>().FromComponentInHierarchy().AsSingle();
            DataInstaller.InstallDatabase(Container, databaseName);
            DataInstaller.Install(Container);
        }


    }
}
