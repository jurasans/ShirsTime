using System;
using System.Linq;
using LiteDB;
using UniRx;
using UnityEngine;

internal class DateSaverService : IDateSave, IDisposable
{
    private LiteDatabase db;
    private LiteRepository repo;
    private LiteCollection<TimeEntry> times;
    public DateTime? CurrentSessionStartTime
    {
        get { return currentSession = currentSession ?? UpdateCurrentSessionState(); }
    }
    private DateTime? currentSession;
    public DateSaverService(LiteDatabase db)
    {
        this.db = db;
        this.repo = new LiteRepository(db);
        times = db.GetCollection<TimeEntry>();
        UpdateCurrentSessionState();

    }

    private DateTime? UpdateCurrentSessionState()
    {
        var currentTimeEntry = repo.Fetch<TimeEntry>()
                    .FirstOrDefault(x => x.EntryTimeStart.HasValue && x.EntryTimeStart.Value.Date == DateTime.Now.Date);
        if (currentTimeEntry != null && currentTimeEntry.EntryTimeStart.HasValue)
        {
            return currentSession = currentTimeEntry.EntryTimeStart.Value;
        }
        return null;
    }

    public IObservable<OperationResult> StartTimer()
    {
        return Observable.Start(() =>
        {
            var sessionStarted = repo.Fetch<TimeEntry>().Exists(x => x.EntryTimeStart.HasValue && x.EntryTimeStart.Value.Date == DateTime.Now.Date);
            if (sessionStarted)
            {
                return OperationResult.SessionInProgress;
            }
            else
            {
                var newDoc = times.Insert(new TimeEntry { EntryTimeStart = DateTime.Now });
                return OperationResult.OK;
            }
        }
        , Scheduler.ThreadPool);
    }

    public void Dispose()
    {
        db.Dispose();
    }

    public IObservable<OperationResult> EnterCustomTime(DateTime start, DateTime end)
    {
        return Observable.Start(() =>
        {
            if ((end - start).TotalHours < 0)
            {
                return OperationResult.EndedBeforeItStarted;
            }
            if (end.Date.DayOfWeek != start.Date.DayOfWeek)
            {
                return OperationResult.DifferenceBetweenDatesTooBig;
            }
            repo.Insert(new TimeEntry { EntryTimeEnd = end, EntryTimeStart = start });
            return OperationResult.OK;
        });
    }

    public IObservable<OperationResult> modifyEntryStartingAt(DateTime startTime)
    {
        throw new NotImplementedException();
    }


    public IObservable<OperationResult> StopTimer()
    {
        return Observable.Start(() =>
        {
            if (!repo.Fetch<TimeEntry>().Exists(x => x.EntryTimeStart.Value.Date == DateTime.Now.Date))
            {
                return OperationResult.NoStartedSession;
            }
            else
            {
                var entry = repo.Fetch<TimeEntry>().First(x => x.EntryTimeStart.Value.Date == DateTime.Now.Date);
                entry.EntryTimeEnd = DateTime.Now;
                return repo.Update(entry) ? OperationResult.OK : OperationResult.NoStartedSession;
            }
        }, Scheduler.ThreadPool);
    }
}

[Serializable]
public class TimeEntry
{
    [BsonId(true)]
    public ObjectId Id { get; set; }
    public DateTime? EntryTimeStart { get; set; }
    public DateTime? EntryTimeEnd { get; set; }
}
