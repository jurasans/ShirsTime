namespace ShirTime.UI
{
    using System.Collections.Generic;
    using ShirTime.General;
    using ShirTime.Services;
    using ShirTime.Settings;
    using UnityEngine;
    using Zenject;

    public class CustomTimeEditorUI : MonoBehaviour, ICustomTimeUI
    {
        [SerializeField]
        private GameObject mainScreenTurnOn;
        [SerializeField]
        private Transform allYourBase;
        public Dictionary<TimeEntry, ITimeEntryView> Views { get; private set; }
        private ViewPool pool;
        private UIDefaultSettings settings;
        [Inject]
        public void Construct(ViewPool pool, UIDefaultSettings settings)
        {
            this.pool = pool;
            this.settings = settings;
            Views = new Dictionary<TimeEntry, ITimeEntryView>();
            mainScreenTurnOn.gameObject.SetActive(false);
        }

        public void Show(bool show)
        {
            mainScreenTurnOn.SetActive(show);
            if (!show)
            {
                Depopulate();
            }
        }
        private void Depopulate()
        {
            foreach (var key in Views.Keys)
            {
                pool.Return((TimeEntryView)Views[key]);
            }
        }

        public void Populate(List<TimeEntry> entriesToShow)
        {
            for (int i = 0; i < entriesToShow.Count; i++)
            {
                var view = pool.Rent();
                view.UpdateData(entriesToShow[i]);
                view.transform.SetParent(allYourBase, false);
                Views[entriesToShow[i]] = view;
            }
        }
    }

}
