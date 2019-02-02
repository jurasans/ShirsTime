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
        private ICustomTimeUI addDifferent;

        public IObservable<Unit> StartTimeClicked { get; private set; }
        public IObservable<Unit> StopTimeClicked { get; private set; }
        public IObservable<Unit> OpenEntryEditorClicked { get; private set; }

        public ReactiveProperty<TimeSpan?> TimeElapsed { get; } = new ReactiveProperty<TimeSpan?>(null);
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
        }
    }

}