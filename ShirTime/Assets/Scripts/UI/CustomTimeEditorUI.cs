namespace ShirTime.UI
{
    using System;
    using System.Collections.Generic;
    using ShirTime.General;
    using ShirTime.Services;
    using ShirTime.Settings;
    using UniRx;
    using UnityEngine;
    using UnityEngine.UI;
    using Zenject;

    public class CustomTimeEditorUI : MonoBehaviour, ICustomTimeUI
    {
        [SerializeField]
        private GameObject mainScreenTurnOn;
        [SerializeField]
        private Transform allYourBase;
        [SerializeField]
        private Button pageForward, pageBack, xButton;
        private ViewPool pool;
        private UIDefaultSettings settings;

        public Dictionary<TimeEntry, ITimeEntryView> Views { get; private set; }
        public IObservable<Unit> PageForward { get; private set; }
        public IObservable<Unit> PageBack { get; private set; }

        [Inject]
        public void Construct(ViewPool pool, UIDefaultSettings settings)
        {
            this.pool = pool;
            this.settings = settings;
            Views = new Dictionary<TimeEntry, ITimeEntryView>();
            mainScreenTurnOn.gameObject.SetActive(false);
            xButton.onClick.AddListener(()=>Show(false));
            PageForward = pageForward.OnClickAsObservable();
            PageBack = pageBack.OnClickAsObservable();
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
