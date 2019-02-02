namespace ShirTime.UI
{
    using System.Collections.Generic;
    using ShirTime.Services;

    internal interface ICustomTimeUI
    {
        void Show(bool show);
        void Populate(List<TimeEntry> entriesToShow);
        Dictionary<TimeEntry, ITimeEntryView> Views { get; }

    }
}
