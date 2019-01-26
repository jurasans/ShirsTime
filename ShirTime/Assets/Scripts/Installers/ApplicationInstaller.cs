using LiteDB;
using UnityEngine;
using Zenject;

public class ApplicationInstaller : MonoInstaller
{
    [SerializeField]
    private string databaseName;
    public override void InstallBindings()
    {
        Container.Bind<IDateSave>().To<DateSaverService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<TimeKeepingManager>().AsSingle().NonLazy();
        Container.Bind<ConnectionString>().FromInstance(new ConnectionString(Application.persistentDataPath+"/"+databaseName+".db"));
    }
}