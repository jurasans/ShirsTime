namespace ShirTime.Installers
{
    using UnityEngine;
    using Zenject;

    public class ApplicationInstaller : MonoInstaller
    {
        [SerializeField]
        private string databaseName;
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<TimeKeepingManager>().AsSingle().NonLazy();
            DataInstaller.InstallDatabase(Container, databaseName);
            DataInstaller.Install(Container);
        }


    }
}