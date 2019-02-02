using System;
using ShirTime.Services;
using UnityEngine;

namespace ShirTime.UI
{
    public interface ITimeEntryView 
    {
        IObservable<DateTime?> EditEnd { get; }
        IObservable<DateTime?> EditStart { get; }
        TimeEntry TimeEntry { get; set; }
    }
}