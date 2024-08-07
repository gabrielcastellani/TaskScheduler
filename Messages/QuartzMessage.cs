using MassTransit.Scheduling;

namespace TaskScheduler.Messages
{
    public class QuartzMessage : ScheduleRecurringMessage
    {
        public Guid CorrelationId => Guid.Parse("41CFD5C0-E6D9-4A88-A83F-83DED0081931");
        public RecurringSchedule Schedule { get; set; }
        public string[] PayloadType { get; set; }
        public Uri Destination { get; set; }
        public object Payload { get; set; }
    }
}
