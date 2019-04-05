using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using NUnit.Framework;
using ShirTime.Services;
using UniRx;
using Zenject;

[TestFixture(Category ="sync")]
public class SyncTests : ZenjectUnitTestFixture
{
	string path = @"C:\Users\ilia\AppData\LocalLow\Lightbringers\ShirTime\2019db.db";
    [Test]
    public void RunTest1()
    {
        using (LiteDB.LiteDatabase db = new LiteDB.LiteDatabase(path))
        {
			var collection = db.GetCollection<TimeEntry>(db.GetCollectionNames().First());
			TestContext.Write(collection.FindAll().First().Id);
			var json = JsonConvert.SerializeObject(collection.FindAll().Take(2));
			var data = Encoding.UTF8.GetBytes(json);
			var what = ObservableWWW.Post("https://localhost:44324/api/values",data, new Dictionary<string,string>(){ {"Content-Type", "application/json" } }).Subscribe();
			Assert.Pass();
			
        }
    }
}