namespace ShirTime.Infra
{
    using System;
    using ShirTime.Services;
    using ShirTime.UI;
    using UniRx;
    using Zenject;

    internal class TimeKeepingManager : IInitializable
    {
        private readonly IMainUICallbacks ui;
        private readonly IDateSave dataService;
        private IObservable<long> intervalDispatcher;
        private IObserver<OperationResult> startObserver;
        private IObserver<OperationResult> stopObserver;

        public TimeKeepingManager(IMainUICallbacks callbacks, IDateSave dataService)
        {
            ui = callbacks;
            this.dataService = dataService;
        }
        public void Initialize()
        {
            ui.StartTimeClicked.Subscribe(x => StartTime());
            ui.StopTimeClicked.Subscribe(x => StopTime());
            stopObserver = Observer.Create<OperationResult>(OnTimerStopped);
            startObserver = Observer.Create<OperationResult>(OnTimerStarted);
            intervalDispatcher = Observable.Interval(TimeSpan.FromMilliseconds(500));
            LazilyUpdateClockIfInSession();
            dataService.SumForCurrentMonth().Subscribe(x => ui.TotalTime.Value = x);
        }

        private void LazilyUpdateClockIfInSession()
        {
            intervalDispatcher.Subscribe(_ =>
            {
                if (dataService.CurrentSessionStartTime.HasValue)
                {
                    ui.TimeElapsed.Value = (dataService.CurrentSessionStartTime.Value - DateTime.Now);
                }
                else
                {
                    ui.TimeElapsed.Value = null;
                }
            });
        }

        private void OnTimerStarted(OperationResult opCode)
        {
            if (opCode != OperationResult.OK)
            {
                ui.ErrorStream.Value = opCode;
            }
        }

        private void OnTimerStopped(OperationResult opCode)
        {
            if (opCode != OperationResult.OK)
            {
                ui.ErrorStream.Value = opCode;
            }
        }

        private void StopTime()
        {
            dataService.StopTimer().Subscribe(stopObserver);
        }

        private void StartTime()
        {
            dataService.StartTimer().Subscribe(startObserver);
        }
    }

}