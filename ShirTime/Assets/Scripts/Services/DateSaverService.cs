using System;
using LiteDB;

internal class DateSaverService : IDateSave , IDisposable
{
    private LiteDatabase db;
    private LiteCollection<TimeEntry> times;

    public DateSaverService(LiteDatabase db)
    {
        this.db = db;

        times = db.GetCollection<TimeEntry>();
    }

    public void EnterCustomTime(DateTime start, DateTime end)
    {
        times.Insert(new TimeEntry(start,end));
    }

    public void modifyEntryStartingAt(DateTime startTime)
    {
    }

    public void StartTimer()
    {
        times.Insert(new TimeEntry(DateTime.Now));
    }

    public void StopTimer()
    {
    }
    public void Dispose()
    {
        db.Dispose();
    }

}

[Serializable]
public class TimeEntry
{
    public ObjectId id;
    public DateTime? EntryTimeStart;
    public DateTime? EntryTimeEnd;

    public TimeEntry (DateTime start)
    {
        EntryTimeStart=start;
    }
    public TimeEntry(DateTime start,DateTime end)
    {
        EntryTimeStart = start;
        EntryTimeEnd=end;
}
}
