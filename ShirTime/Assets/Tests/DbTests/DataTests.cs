﻿using Zenject;
using NUnit.Framework;
using ShirTime.Installers;
using LiteDB;
using System;
using System.Linq;
using UniRx;
using System.Threading.Tasks;

[TestFixture]
public class DataTests : ZenjectUnitTestFixture
{
    [Inject]
    IDateSave dataSave;
    [Inject]
    LiteRepository repo;
    public override void Setup()
    {
        base.Setup();
        DataInstaller.InstallDatabase(Container);
        DataInstaller.Install(Container);
        Container.Inject(this);
    }

    [Test(Author ="ilia" , Description ="will check all the edge cases for starting a session")]
    public void StartSession()
    {
        //todo:will check if there is an entry for today. if there is then it will not insert new data entry.
        var result = dataSave.StartTimer().Wait();
        var timeEntry = repo.Fetch<TimeEntry>().OrderBy(x => x.EntryTimeStart).First();
        Assert.True(timeEntry.EntryTimeStart.HasValue && !timeEntry.EntryTimeEnd.HasValue, "added wrong data");
        Assert.True((timeEntry.EntryTimeStart.Value - DateTime.Now).Seconds < 2, "too much time passed between start and test.");
        Assert.True(result == OperationResult.OK);
    }
    [Test]
    public void StopSession()
    {
        dataSave.StartTimer().Wait();
        var sequence = dataSave.StopTimer().Wait();
        Assert.True(sequence == OperationResult.OK, "sequence end Operation state is Incorrect");
        var entry = repo.Fetch<TimeEntry>().FindLast(x => true);
        Assert.True(entry.EntryTimeEnd.HasValue && entry.EntryTimeStart.HasValue, "data entry was not totally successful");

    }
    [Test]
    public void StartSessionOnTopOfSession()
    {
        dataSave.StartTimer().Wait();
        var result = dataSave.StartTimer().Wait();

        Assert.True(result == OperationResult.SessionInProgress, $"wrong operation result after starting timer for the second time. expected:{OperationResult.SessionInProgress}, got : {result}");
        Assert.True(repo.Fetch<TimeEntry>().Count() == 1, "more then one entry was found where 1 was required.");

    }
    [Test]
    public void GetDayEntry()
    {
        dataSave.StartTimer().Wait();
        Assert.True( dataSave.CurrentSessionStartTime.HasValue , "session started but not recognized");
        dataSave.StopTimer().Wait();
        Assert.False(dataSave.CurrentSessionStartTime.HasValue);
    }
    [Test]
    public void StartedNewSessionAfterClosing()
    {
        dataSave.StartTimer().Wait();
        dataSave.StopTimer().Wait();
        Assert.True(repo.Fetch<TimeEntry>().Count() == 1, "more then one entry was found where 1 was required.");
        var result = dataSave.StartTimer().Wait();

        Assert.True(result == OperationResult.OK, $"returned {result} for starting new session at the same day.");
        Assert.True(repo.Fetch<TimeEntry>().Count()==2, "newly started session in the same day, did not open.");
        Assert.True(repo.Fetch<TimeEntry>().All(x=>x.EntryTimeStart.HasValue) && !repo.Fetch<TimeEntry>().All(x=>x.EntryTimeEnd.HasValue),"did not make sure starting a new session would no close both for the day.");
        
    }
    [Test]
    public void StopSessionWithoutStarting()
    {
        var stop = dataSave.StopTimer().Wait();
        Assert.True(repo.Fetch<TimeEntry>().FindAll(x => true).Count == 0, "found entry where there shouldnt be any");
        Assert.True(stop == OperationResult.NoStartedSession, $"operation result unexcpected.is:{stop}");

    }

    [Test]
    public void DifferenceBetweenCustomDatesTooLarge()
    {
        //todo: will check if a custom parameter actually gets inserted. if the same day is inserted then it will update that day's entry.
        var tooBigDifference = dataSave.EnterCustomTime(DateTime.MinValue, DateTime.Now).Wait();
        Assert.True(tooBigDifference == OperationResult.DifferenceBetweenDatesTooBig);

    }

    [Test]
    public void EndedBeforeItStartedCustomDateTest()
    {
        var endedBeforeStarted = dataSave.EnterCustomTime(DateTime.MaxValue, DateTime.Now).Wait();
        Assert.True(repo.Fetch<TimeEntry>().FindAll(x => true).Count == 0, "found entry where there shouldnt be any");
        Assert.True(endedBeforeStarted == OperationResult.EndedBeforeItStarted);
    }
    //todo
    [Test]
    public void EnterValidCustomDate()
    {
        var customDate = dataSave.EnterCustomTime(DateTime.Now, DateTime.Now.AddMilliseconds(5)).Wait();
        Assert.True(repo.Fetch<TimeEntry>().FindAll(x => true).Count > 0, "didnt find entry where there should be");
        Assert.True(customDate == OperationResult.OK);
    }

    public override void Teardown()
    {
        repo.Database.DropCollection("TimeEntry");
        base.Teardown();

    }

    [OneTimeTearDown]
    public void TearDownTests()
    {
        repo.Database.Dispose();
    }
}