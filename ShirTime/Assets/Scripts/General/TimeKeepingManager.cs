using Zenject;
using UniRx;
using System;

internal class TimeKeepingManager : IInitializable
{
    private readonly IMainUICallbacks ui;
    private readonly IDateSave dataService;

    public TimeKeepingManager(IMainUICallbacks callbacks,IDateSave dataService)
    {
        ui = callbacks;
        this.dataService = dataService;
    }
    public void Initialize()
    {
        ui.StartTimeClicked.Subscribe(x=>StartTime());
        ui.StopTimeClicked.Subscribe(x=>StopTime());
        
    }

    private void StopTime()
    {
        dataService.StopTimer();
    }

    private void StartTime()
    {
        dataService.StartTimer();
    }
}