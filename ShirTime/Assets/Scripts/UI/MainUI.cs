namespace ShirTime.UI
{
    using System;
    using ShirTime.Services;
    using UniRx;
    using UnityEngine;
    using UnityEngine.UI;
    using Zenject;

    public class MainUI : MonoBehaviour, IMainUICallbacks
    {
        [SerializeField]
        private Button startTime, stopTime, openCustomTimePanel;
        [SerializeField]
        private Text status;
        [SerializeField]
        private Text total;

        private ICustomTimeUI addDifferent;

        public IObservable<Unit> StartTimeClicked { get; private set; }
        public IObservable<Unit> StopTimeClicked { get; private set; }
        public IObservable<Unit> OpenEntryEditorClicked { get; private set; }

        public ReactiveProperty<TimeSpan?> TimeElapsed { get; } = new ReactiveProperty<TimeSpan?>(null);
        public ReactiveProperty<TimeSpan?> TotalTime { get; } = new ReactiveProperty<TimeSpan?>(null);

        public ReactiveProperty<OperationResult> ErrorStream { get; private set; }
        [Inject]
        void Construct(ICustomTimeUI addDifferent)
        {
            ErrorStream = new ReactiveProperty<OperationResult>();
            this.addDifferent = addDifferent;
            StartTimeClicked = startTime.OnClickAsObservable();
            StopTimeClicked = stopTime.OnClickAsObservable();
            OpenEntryEditorClicked = openCustomTimePanel.OnClickAsObservable();
            TimeElapsed.Subscribe(t => status.text = (t.HasValue ? "You are Doing Great!\n" + t.Value.ToString(@"hh\:mm\:ss") : "Waiting For Session Start."));
            TotalTime.ObserveOnMainThread().Subscribe(t => total.text = SetUiTotalTimeText(t));
        }

        private string SetUiTotalTimeText(TimeSpan? totalTime)
        {
            TimeSpan timeToDisplay = TimeSpan.Zero;
            if (totalTime.HasValue)
            {
                timeToDisplay += totalTime.Value;
            }
            if (TimeElapsed.Value.HasValue)
            {
                timeToDisplay += TimeElapsed.Value.Value;
            }
            return timeToDisplay.Equals(TimeSpan.Zero)
                ? "No time has been logged"
                : $"Working for {timeToDisplay.TotalHours:F2} hours this month.\n(or {timeToDisplay.TotalDays:F2} whole days)";
        }
    }

}