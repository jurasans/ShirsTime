namespace ShirTime.UI
{
    using System.Collections.Generic;
    using ShirTime.General;
    using ShirTime.Services;
    using ShirTime.Settings;
    using UnityEngine;
    using Zenject;

    public class AddDifferentTimeUI : MonoBehaviour, ICustomTimeUI
    {
        [SerializeField]
        private GameObject mainScreenTurnOn;
        [SerializeField]
        private Transform allYourBase;
        private Dictionary<TimeEntry, TimeEntryView> views;
        private ViewPool pool;
        private UIDefaultSettings settings;
        [Inject]
        public void Construct(ViewPool pool, UIDefaultSettings settings)
        {
            this.pool = pool;
            this.settings = settings;
            views = new Dictionary<TimeEntry, TimeEntryView>();
            mainScreenTurnOn.gameObject.SetActive(false);
        }
        public void Show(bool show)
        {
            mainScreenTurnOn.SetActive(show);
        }

        public void Populate(List<TimeEntry> entriesToShow)
        {
            for (int i = 0; i < entriesToShow.Count; i++)
            {
                var view = pool.Rent();
                view.UpdateData(entriesToShow[i]);
                view.transform.SetParent(allYourBase, false);
                views[entriesToShow[i]] = view;
            }
        }
    }

}
