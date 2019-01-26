using Zenject;
using NUnit.Framework;
using ShirTime.Installers;
using LiteDB;
using System;
using System.Linq;
using System.Collections.Generic;
[TestFixture]
public class DataTests : ZenjectUnitTestFixture
{
    

    [SetUp]
    public void SetupTest()
    {
        DataInstaller.InstallDatabase(Container);
        DataInstaller.Install(Container);
    }
    [Inject]
    IDateSave dataSave;
    [Inject]
    LiteDB.LiteRepository repo;

    [Test]
    public void StartSession()
    {
        //todo:will check if there is an entry for today. if there is then it will not insert new data entry.

        dataSave.StartTimer();
        //var timeEntry = repo.Fetch<TimeEntry>().OrderBy(x=>x.EntryTimeStart).First();
        //Assert.True(timeEntry.HasValue && !timeEntry.EntryTimeEnd.HasValue,"added wrong data");
        //Assert.True((timeEntry.EntryTimeStart.Value-DateTime.Now).Seconds<2,"too much time passed between start and test.");
    }
    [Test]
    public void StopSession()
    {
        //todo:will check if there is an entry for today. if there is then it will insert new date in the enddate..
        //if not , think about what happens... maybe some result type for operation needs to be returned for better ui indications and other stuff.

        dataSave.StopTimer();
        //var timeEntry = repo.Fetch<TimeEntry>().Max(x=>x.EntryTimeStart.Value);
        //Assert.True(timeEntry.EntryTimeStart.HasValue && !timeEntry.EntryTimeEnd.HasValue, "added wrong data");
        //Assert.True((timeEntry.EntryTimeStart.Value - DateTime.Now).Seconds < 2, "too much time passed between start and test.");
    }
    
    [Test]
    public void EnterCustomDate()
    {
        //todo: will check if a custom parameter actually gets inserted. if the same day is inserted then it will update that day's entry.
    }
    [TearDown]
    public void TearDown()
    {
    }
}
