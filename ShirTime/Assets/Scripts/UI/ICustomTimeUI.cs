namespace ShirTime.UI
{
    using System;
    using System.Collections.Generic;
    using ShirTime.Services;
    using UniRx;

    internal interface ICustomTimeUI
    {
        void Show(bool show);
        void Populate(List<TimeEntry> entriesToShow);
        Dictionary<TimeEntry, ITimeEntryView> Views { get; }
		IObservable<Unit> PageForward { get;}
        IObservable<Unit> PageBack { get; }
    }
}
