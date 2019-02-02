namespace ShirTime.Infra
{
    using System;
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
                ui.Populate(new System.Collections.Generic.List<TimeEntry>
                {
                    new TimeEntry
                    {
                        Id=new LiteDB.ObjectId(),
                        EntryTimeStart=DateTime.Now,
                        EntryTimeEnd= DateTime.Now.Add(TimeSpan.FromSeconds(1))
                    }
                });
            });
        }


    }
}
