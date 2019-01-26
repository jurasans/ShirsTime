namespace ShirTime.Installers
{
    using LiteDB;
    using UnityEngine;
    using Zenject;

    public class DataInstaller : Installer<DataInstaller>
    {
        private const string TEST_DATABASE_NAME = "tstdb";

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<DateSaverService>().AsSingle().NonLazy();
        }
        public static void InstallDatabase(DiContainer container, string name = null)
        {
            ConnectionString conString;
            if (string.IsNullOrEmpty(name))
            {
                conString =new ConnectionString(Application.persistentDataPath + "/" + TEST_DATABASE_NAME + ".db");

            }
            else
            {
                conString=new ConnectionString(Application.persistentDataPath + "/" + name + ".db");

            }
            var db = new LiteDatabase(conString);
            var repo = new LiteRepository(db);
            container.BindInstance(db).AsSingle();
            container.BindInstance(repo).AsSingle();


        }
    } 
}
