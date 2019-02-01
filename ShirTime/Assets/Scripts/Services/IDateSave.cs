using System;

public interface IDateSave
{
    DateTime? CurrentSessionStartTime { get; }

    IObservable<OperationResult> StartTimer();
    IObservable<OperationResult> StopTimer();
    IObservable<OperationResult> EnterCustomTime(DateTime start, DateTime end);
    IObservable<OperationResult> modifyEntryStartingAt(DateTime startTime);

}
public enum OperationResult
{
    OK,
    SessionInProgress,
    NoStartedSession,
    DifferenceBetweenDatesTooBig,
    EndedBeforeItStarted
}
