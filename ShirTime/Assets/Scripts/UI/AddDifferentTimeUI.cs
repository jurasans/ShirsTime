namespace ShirTime.UI
{
    using System.Collections.Generic;
    using ShirTime.General;
    using ShirTime.Services;
    using ShirTime.Settings;
    using UnityEngine;

    public class AddDifferentTimeUI : MonoBehaviour, ICustomTimeUI
    {
        [SerializeField]
        private GameObject mainScreenTurnOn;
        [SerializeField]
        private readonly Transform allYourBase;
        private List<TimeEntryView> views;
        private TimeEntryViewMemomoryPool pool;
        private UIDefaultSettings settings;
        private List<TimeEntry> entries;

        public void Construct(TimeEntryViewMemomoryPool pool, UIDefaultSettings settings)
        {
            this.pool = pool;
            this.settings = settings;
            entries = new List<TimeEntry>();
            views = new List<TimeEntryView>();
        }
        public void Show(bool show)
        {
            mainScreenTurnOn.SetActive(show);
        }

        public void Populate(List<TimeEntry> entriesToShow)
        {


        }
    }

}
