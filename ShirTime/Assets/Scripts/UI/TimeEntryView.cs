namespace ShirTime.UI
{
    using System;
    using ShirTime.Services;
    using UniRx;
    using UnityEngine;
    using UnityEngine.UI;

    public class TimeEntryView : MonoBehaviour
    {
        public Button startEdit, endEdit;
        public Text startT, endT, summeryT;
        private IObservable<DateTime?> editStart;
        private IObservable<DateTime?> editEnd;

        public TimeEntry TimeEntry { get; set; }
        private void Start()
        {
            editStart = startEdit.OnClickAsObservable().Select(x => TimeEntry.EntryTimeStart);
            editEnd = startEdit.OnClickAsObservable().Select(x => TimeEntry.EntryTimeEnd);

        }
        internal void UpdateData(TimeEntry timeEntry)
        {
            TimeEntry = timeEntry;
            startT.text = TimeEntry.EntryTimeStart.ToString();
            endT.text = TimeEntry.EntryTimeEnd.ToString();
            summeryT.text = (TimeEntry.EntryTimeEnd - TimeEntry.EntryTimeStart).ToString();
        }
    }

}