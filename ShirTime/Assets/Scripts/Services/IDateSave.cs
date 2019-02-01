using System;

public interface IDateSave
{
    DateTime? CurrentSessionStartTime { get; }
    TimeEntry CurrentOpenSession { get;}
    IObservable<OperationResult> StartTimer();
    IObservable<OperationResult> StopTimer();
    IObservable<Tuple<OperationResult,TimeEntry>> EnterNewCustomTimeEntry(DateTime start, DateTime end);
    IObservable<OperationResult> ModifyEntry(TimeEntry entry, DateTime start, DateTime end);
}
public enum OperationResult
{
    OK,
    SessionInProgress,
    NoStartedSession,
    DifferenceBetweenDatesTooBig,
    EndedBeforeItStarted,
    UnexcpectedError
}
