using MassTransit;
using TaskScheduler.Messages;
using TaskScheduler.Schedules;

namespace TaskScheduler.Background
{
    internal sealed class MessageTriggerBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public MessageTriggerBackgroundService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await SendQuartzMessage(stoppingToken);
        }

        private async Task SendQuartzMessage(CancellationToken cancellationToken)
        {
            AsyncServiceScope scope = default;

            try
            {
                scope = _serviceScopeFactory.CreateAsyncScope();

                var messageScheduler = scope.ServiceProvider.GetRequiredService<IMessageScheduler>();
                var quartzMessage = new QuartzMessage
                {
                    Destination = new Uri("queue:quartz"),
                    Payload = new { Name = "Gabriel" },
                    PayloadType = new string[1] { "Person" },
                    Schedule = new EachMinute(),
                };

                await messageScheduler.SchedulePublish(
                    scheduledTime: new DateTime() + TimeSpan.FromSeconds(10),
                    message: quartzMessage,
                    cancellationToken: cancellationToken);
            }
            finally
            {
                scope.Dispose();
            }
        }

        private async Task SendScheduleMessage(CancellationToken cancellationToken)
        {
            AsyncServiceScope scope = default;

            try
            {
                scope = _serviceScopeFactory.CreateAsyncScope();

                var bus = scope.ServiceProvider.GetRequiredService<IBus>();
                var schedulerEndpoint = await bus.GetSendEndpoint(new Uri("queue:quartz"));

                await schedulerEndpoint.ScheduleRecurringSend(
                    destinationAddress: new Uri("queue:quartz"),
                    schedule: new MessageSchedule(),
                    message: new Message("Hello World"),
                    cancellationToken: cancellationToken);
            }
            finally
            {
                scope.Dispose();
            }
        }

        private async Task SendMessage(CancellationToken cancellationToken)
        {
            AsyncServiceScope scope = default;

            try
            {
                scope = _serviceScopeFactory.CreateAsyncScope();

                var messageScheduler = scope.ServiceProvider.GetRequiredService<IMessageScheduler>();
                await messageScheduler.SchedulePublish(
                    scheduledTime: new DateTime() + TimeSpan.FromSeconds(10),
                    message: new Message("Hello World!"),
                    cancellationToken: cancellationToken);
            }
            finally
            {
                scope.Dispose();
            }
        }
    }
}
