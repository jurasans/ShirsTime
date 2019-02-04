using System;
using ShirTime.Services;

namespace ShirTime.UI
{
    public interface ITimeEntryView
    {
        IObservable<DateTime?> EditEnd { get; }
        IObservable<DateTime?> EditStart { get; }
        TimeEntry TimeEntry { get; set; }
        void UpdateData(TimeEntry timeEntry);
    }
}