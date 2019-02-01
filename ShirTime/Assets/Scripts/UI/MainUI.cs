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

    public ReactiveProperty<TimeSpan?> TimeElapsed { get; } = new ReactiveProperty<TimeSpan?>();
    public ReactiveProperty<OperationResult> ErrorStream { get; } = new ReactiveProperty<OperationResult>();
    [Inject]
    void Construct(ICustomTimeUI addDifferent)
    {
        this.addDifferent = addDifferent;
    }
    void Start()
    {
        StartTimeClicked = startTime.OnClickAsObservable();
        StopTimeClicked = stopTime.OnClickAsObservable();
        openCustomTimePanel.OnClickAsObservable().Subscribe(x => addDifferent.Show(true));
        TimeElapsed.Subscribe(t => status.text = "You are Doing Great!\n" + (t.HasValue ? t.Value.ToString("hh:mm:ss") : ""));
    }
}
internal class PopupSystem
{
    private IMainUICallbacks uiCallbacks;

    public PopupSystem(IMainUICallbacks uiCallbacks)
    {
        this.uiCallbacks = uiCallbacks;
        uiCallbacks.ErrorStream.Subscribe(Observer.Create<OperationResult>(ShowError));
    }

    private void ShowError(OperationResult error)
    {
        throw new NotImplementedException(error.ToString());
    }
}
