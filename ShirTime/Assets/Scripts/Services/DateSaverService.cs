using System;
using System.Collections.Generic;
using LiteDB;

internal class DateSaverService : IDateSave
{
    private LiteDatabase db;
    private LiteCollection<TimeEntry> times;

    public DateSaverService(ConnectionString conString)
    {
        db = new LiteDB.LiteDatabase(conString);

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
    }

    public void StopTimer()
    {
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
