namespace ShirTime.Infra
{
    using System;
    using System.Collections.Generic;
    using FantomLib;
    using ShirTime.Services;
    using ShirTime.UI;
    using UniRx;
    using UnityEngine;
    using Zenject;

    internal class EntryEditor : IInitializable
    {
        private readonly ICustomTimeUI ui;
        private readonly IDateSave dataService;
        private readonly TimePickerController timePicker;
        private readonly DatePickerController datePicker;
        private IObservable<string> newTimeObs;
        private int page = 0;
        public EntryEditor(
            ICustomTimeUI ui,
            IMainUICallbacks mainUi,
            IDateSave dataService,
            TimePickerController timePicker,
            DatePickerController datePicker)
        {
            this.ui = ui;
            this.dataService = dataService;
            this.timePicker = timePicker;
            this.datePicker = datePicker;
            mainUi.OpenEntryEditorClicked.Subscribe(x =>
            {
                ui.Show(true);
                dataService.GetAllEntries(0, 5).ObserveOnMainThread().Subscribe(UpdateUI);
            });
        }

        public void UpdateUI(List<TimeEntry> entries)
        {
            ui.Populate(entries);
            RegisterViewsCallbacks(entries);
        }

        private void RegisterViewsCallbacks(List<TimeEntry> entries)
        {
            for (int i = 0; i < entries.Count; i++)
            {
                var view = ui.Views[entries[i]];
                view.EditEnd
                    .Do(date => timePicker.Show(date.Value.ToString(@"HH\:mm")))
                    .CombineLatest(newTimeObs, OnEditedTimeInPicker)
                    .Subscribe(d => UpdateViewAndData(view, d).Subscribe(x => Debug.Log(x)));

                view.EditStart //get the current time
                    .Do(date => timePicker.Show(date.Value.ToString(@"HH\:mm"))) //show and put it in picker
                    .CombineLatest(newTimeObs, OnEditedTimeInPicker) // wait for a result from picker.-> parse it into the date context of the entry.
                    .Subscribe(d => UpdateViewAndData(view, d).Subscribe(x => Debug.Log("updated entry: " + x)));//update entry and view.


                view.UpdateData(view.TimeEntry);
            }
        }

        private IObservable<OperationResult> UpdateViewAndData(ITimeEntryView view, DateTime v)
        {
            view.TimeEntry.EntryTimeEnd = v;
            view.UpdateData(view.TimeEntry);
            return dataService.ModifyEntry(view.TimeEntry, view.TimeEntry.EntryTimeStart.Value, view.TimeEntry.EntryTimeEnd.Value);
        }

        private DateTime OnEditedTimeInPicker(DateTime? defaultTime, string fromPicker)
        {
            var time = TimeSpan.Parse(fromPicker);
            return defaultTime.Value.Date.Add(time);
        }

        public void Initialize()
        {
            ui.PageBack.ContinueWith(_ =>
            {
                ui.Depopulate();
                return dataService.GetAllEntries(page = Mathf.Clamp(page--, 0, 99), 5).ObserveOnMainThread();
            }
            ).Subscribe(UpdateUI); ;
            ui.PageForward.ContinueWith(_=>
            {
                ui.Depopulate();
                return dataService.GetAllEntries(page = Mathf.Clamp(page++, 0, 99), 5).ObserveOnMainThread();
            }
            ).Subscribe(UpdateUI); ;
#if !UNITY_EDITOR
            newTimeObs = timePicker.OnResult.AsObservable();
#else
            newTimeObs = Observable.Interval(TimeSpan.FromSeconds(1)).Select(_ => "23:23");
#endif
        }
    }
}
