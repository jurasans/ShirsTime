using System;

public interface IDateSave
{
    void StartTimer();
    void StopTimer();
    void EnterCustomTime(DateTime start,DateTime end);
    void modifyEntryStartingAt(DateTime startTime);

}
