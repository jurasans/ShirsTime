using System.IO;
using LiteDB;
using ShirTime.Services;
using UniRx;

namespace ShirTime.Installers
{
    internal class DataSync : IDataSync
    {
        private DataSyncConnectionString connection;
		string filePath;

        public DataSync(DataSyncConnectionString connection , ConnectionString localDbConnection)
        {
			this.connection = connection;
            filePath = localDbConnection.Filename;
        }
	
        public void SendDbToServer()
        {
			ObservableWWW.Post(connection.url+connection.endPoint,File.ReadAllBytes(filePath)).Subscribe();
        }
    }
}