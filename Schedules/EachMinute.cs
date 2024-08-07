using MassTransit.Scheduling;

namespace TaskScheduler.Schedules
{
    public class EachMinute : RecurringSchedule
    {
        public string TimeZoneId { get; set; }
        public DateTimeOffset StartTime => DateTimeOffset.Now;
        public DateTimeOffset? EndTime { get; set; }
        public string ScheduleId => "EachMinute";
        public string ScheduleGroup => "Group01";
        public string CronExpression => "0 0/1 * 1/1 * ? *";
        public string Description => "Each minute";
        public MissedEventPolicy MisfirePolicy => MissedEventPolicy.Skip;
    }
}
