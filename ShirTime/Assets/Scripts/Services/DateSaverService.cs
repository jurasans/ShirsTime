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

    public TimeEntry CurrentOpenSession { get;private set;}

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
        var currentTimeEntry = GetOpenSession();
        if (currentTimeEntry != null && currentTimeEntry.EntryTimeStart.HasValue)
        {
            return currentSession = currentTimeEntry.EntryTimeStart.Value;
        }
        return null;
    }

    public TimeEntry GetOpenSession()
    {
                
        return CurrentOpenSession = repo.Fetch<TimeEntry>()
                            .FirstOrDefault(x =>
                            x.EntryTimeStart.HasValue
                            && x.EntryTimeStart.Value.Date == DateTime.Now.Date
                            && !x.EntryTimeEnd.HasValue);
    }

    public IObservable<OperationResult> StartTimer()
    {
        return Observable.Start(() =>
        {
            var sessionStarted = GetOpenSession();
            if (sessionStarted != null)
            {
                return OperationResult.SessionInProgress;
            }
            else
            {
                var newDoc = times.Insert(CurrentOpenSession= new TimeEntry { EntryTimeStart = DateTime.Now });
                CurrentOpenSession.Id= newDoc;
                return OperationResult.OK;
            }
        }
        , Scheduler.ThreadPool);
    }

    public void Dispose()
    {
        db.Dispose();
    }

    public IObservable<Tuple<OperationResult, TimeEntry>> EnterNewCustomTimeEntry(DateTime start, DateTime end)
    {
        TimeEntry entry = null;

        return Observable.Start(() =>
        {
            if ((end - start).TotalHours < 0)
            {
                return new Tuple<OperationResult, TimeEntry>(OperationResult.EndedBeforeItStarted, null);
            }
            if (end.Date.DayOfWeek != start.Date.DayOfWeek)
            {
                return new Tuple<OperationResult, TimeEntry>(OperationResult.DifferenceBetweenDatesTooBig, null);
            }
            var id = repo.Insert<TimeEntry>(entry = new TimeEntry
            {
                EntryTimeEnd = end,
                EntryTimeStart = start
            });

            entry.Id = id;
            return new Tuple<OperationResult, TimeEntry>(OperationResult.OK, entry);
        }, Scheduler.ThreadPool);
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
                currentSession = null;
                return repo.Update(entry) ? OperationResult.OK : OperationResult.NoStartedSession;
            }
        }, Scheduler.ThreadPool);
    }

    public IObservable<OperationResult> ModifyEntry(TimeEntry entry, DateTime newStart, DateTime newEnd)
    {
        return Observable.Start(() =>
        {
            entry.EntryTimeStart = newStart;
            entry.EntryTimeEnd = newEnd;
            return repo.Update(entry) ? OperationResult.OK : OperationResult.UnexcpectedError;
        }
        , Scheduler.ThreadPool);
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
