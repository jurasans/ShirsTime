namespace ShirTime.Services
{
    using System;

    public interface IDateSave
    {
        TimeEntry GetOpenSession();
        DateTime? CurrentSessionStartTime { get; }
        IObservable<OperationResult> StartTimer();
        IObservable<OperationResult> StopTimer();
        IObservable<Tuple<OperationResult, TimeEntry>> EnterNewCustomTimeEntry(DateTime start, DateTime end);
        IObservable<OperationResult> ModifyEntry(TimeEntry entry, DateTime start, DateTime end);
        IObservable<TimeSpan> SumForCurrentMonth();
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
}