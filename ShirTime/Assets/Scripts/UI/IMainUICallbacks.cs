namespace ShirTime.UI
{
    using System;
    using ShirTime.Services;
    using UniRx;

    internal interface IMainUICallbacks
    {
        IObservable<Unit> StartTimeClicked { get; }
        IObservable<Unit> StopTimeClicked { get; }
        IObservable<Unit> OpenEntryEditorClicked { get; }
        ReactiveProperty<TimeSpan?> TimeElapsed { get; }
        ReactiveProperty<OperationResult> ErrorStream { get; }
    }

}