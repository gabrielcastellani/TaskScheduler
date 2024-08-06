using MassTransit.Scheduling;

namespace TaskScheduler.Schedules
{
    public class MessageSchedule : DefaultRecurringSchedule
    {
        public MessageSchedule()
        {
            CronExpression = "0 0/1 * 1/1 * ? *";
        }
    }
}
