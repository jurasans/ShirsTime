using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using UniRx;
using System;

public class MainUI : MonoBehaviour, IMainUICallbacks 
{
    [SerializeField]
    private Button startTime, stopTime, openCustomTimePanel;
    [SerializeField]
    private Text status;
    private ICustomTimeUI addDifferent;

    public IObservable<Unit> StartTimeClicked { get; private set; }
    public IObservable<Unit> StopTimeClicked { get; private set; }

    public ReactiveProperty<TimeSpan?> TimeElapsed { get; } = new ReactiveProperty<TimeSpan?>(null);
    public ReactiveProperty<OperationResult> ErrorStream { get; private set;} 
    [Inject]
    void Construct(ICustomTimeUI addDifferent)
    {
        ErrorStream = new ReactiveProperty<OperationResult>();
        this.addDifferent = addDifferent;
        StartTimeClicked = startTime.OnClickAsObservable();
        StopTimeClicked = stopTime.OnClickAsObservable();
        openCustomTimePanel.OnClickAsObservable().Subscribe(x => addDifferent.Show(true));
        TimeElapsed.Subscribe(t => status.text = (t.HasValue ? "You are Doing Great!\n"+ t.Value.ToString(@"hh\:mm\:ss") : "Waiting For Session Start."));
    }
}
