namespace ShirTime.Infra
{
    using System;
    using System.Collections.Generic;
    using ShirTime.Services;
    using ShirTime.UI;
    using UniRx;

    internal class EntryEditor
    {
        private readonly ICustomTimeUI ui;
        private readonly IDateSave dataService;

        public EntryEditor(
            ICustomTimeUI ui,
            IMainUICallbacks mainUi,
            IDateSave dataService)
        {
            this.ui = ui;
            this.dataService = dataService;
            mainUi.OpenEntryEditorClicked.Subscribe(x =>
            {
                ui.Show(true);
                //mockit
                var entries = new System.Collections.Generic.List<TimeEntry>
                {
                    new TimeEntry
                    {
                        Id=new LiteDB.ObjectId(),
                        EntryTimeStart=DateTime.Now,
                        EntryTimeEnd= DateTime.Now.Add(TimeSpan.FromSeconds(1))
                    }
                };
                UpdateUI(entries);
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
                view.EditStart.Subscribe(x => { view.TimeEntry.EntryTimeStart = EditField(x); view.UpdateData(view.TimeEntry); });
                view.EditEnd.Subscribe(x => { view.TimeEntry.EntryTimeEnd = EditField(x); view.UpdateData(view.TimeEntry); });
            }
        }

        private DateTime? EditField(DateTime? x)
        {

            return DateTime.Now;
        }
    }
}
