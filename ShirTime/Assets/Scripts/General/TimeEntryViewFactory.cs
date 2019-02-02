namespace ShirTime.General
{
    using ShirTime.Services;
    using ShirTime.UI;
    using Zenject;

    public class TimeEntryViewFactory : IFactory<TimeEntry, TimeEntryView>
    {
        public TimeEntryView Create(TimeEntry param)
        {
            return null;
        }
    }

}