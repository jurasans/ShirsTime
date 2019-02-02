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
        public Text startT, endT, summeryT;
        public IObservable<DateTime?> EditStart { get; private set; }
        public IObservable<DateTime?> EditEnd { get; private set; }
        public TimeEntry TimeEntry { get; set; }

        internal void UpdateData(TimeEntry timeEntry)
        {
            TimeEntry = timeEntry;
            EditStart = startEdit.OnClickAsObservable().Select(x => TimeEntry.EntryTimeStart);
            EditEnd = startEdit.OnClickAsObservable().Select(x => TimeEntry.EntryTimeEnd);
            startT.text = TimeEntry.EntryTimeStart.ToString();
            endT.text = TimeEntry.EntryTimeEnd.ToString();
            summeryT.text = (TimeEntry.EntryTimeEnd - TimeEntry.EntryTimeStart).ToString();
        }
    }

}