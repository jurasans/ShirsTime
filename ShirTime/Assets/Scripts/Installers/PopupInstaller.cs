namespace ShirTime.Installers
{
    using Zenject;

    public class PopupInstaller: MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<PopupSystem>().AsSingle().NonLazy();
        }
    }
}
