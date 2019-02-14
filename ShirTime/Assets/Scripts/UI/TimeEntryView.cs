namespace ShirTime.UI
{
    using System;
    using ShirTime.Services;
    using UniRx;
    using UnityEngine;
    using UnityEngine.UI;

    public class TimeEntryView : MonoBehaviour, ITimeEntryView
    {
        public Button startEdit, endEdit;
        public Text dateT, startT, endT, summeryT;
        public IObservable<DateTime?> EditStart { get; private set; }
        public IObservable<DateTime?> EditEnd { get; private set; }
        public TimeEntry TimeEntry { get; set; }
        private void Awake()
        {
            EditStart = startEdit.OnClickAsObservable().Select(x => TimeEntry.EntryTimeStart);
            EditEnd = endEdit.OnClickAsObservable().Select(x => TimeEntry.EntryTimeEnd);
        }
        public void UpdateData(TimeEntry timeEntry)
        {
            TimeEntry = timeEntry;
            startT.text = TimeEntry.EntryTimeStart.Value.ToShortTimeString();
            endT.text = TimeEntry.EntryTimeEnd.Value.ToShortTimeString();
            summeryT.text = (TimeEntry.EntryTimeEnd - TimeEntry.EntryTimeStart).Value.ToString(@"hh\:mm");
            dateT.text = timeEntry.EntryTimeStart.Value.ToShortDateString();
        }
    }

}