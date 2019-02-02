namespace ShirTime.Infra
{
    using ShirTime.Services;
    using ShirTime.UI;
    using Zenject;

    public class TimeEntryViewMemomoryPool : MemoryPool<TimeEntry, TimeEntryView>
    {
        public TimeEntryViewMemomoryPool()
        {
        }
        protected override void Reinitialize(TimeEntry p1, TimeEntryView item)
        {
            item.ResetView(p1);
        }


    }

}