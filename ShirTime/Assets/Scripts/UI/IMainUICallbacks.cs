using UniRx;
using System;
internal interface IMainUICallbacks
{
    IObservable<Unit> StartTimeClicked { get; }
    IObservable<Unit> StopTimeClicked { get; }
    ReactiveProperty<TimeSpan?> TimeElapsed { get; }
    ReactiveProperty<OperationResult> ErrorStream { get; }
}
