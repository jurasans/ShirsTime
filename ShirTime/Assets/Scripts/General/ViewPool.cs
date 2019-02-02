using ShirTime.UI;
using UniRx.Toolkit;
using UnityEngine;

namespace ShirTime.General
{
    public class ViewPool : ObjectPool<TimeEntryView>
    {
        private readonly TimeEntryView prefab;

        public ViewPool(TimeEntryView prefab)
        {
            this.prefab = prefab;
        }

        protected override TimeEntryView CreateInstance()
        {
            return GameObject.Instantiate(prefab);
        }

    }
}
