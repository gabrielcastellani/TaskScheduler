using MassTransit;
using TaskScheduler.Messages;
using TaskScheduler.Schedules;

namespace TaskScheduler.Background
{
    internal sealed class MessageTriggerBackgroundService : BackgroundService
    {
        private readonly IBus _bus;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public MessageTriggerBackgroundService(
            IBus bus,
            IServiceScopeFactory serviceScopeFactory)
        {
            _bus = bus;
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await SendQuartzMessageAsync(stoppingToken);
            await SendEmailMessageAsync(stoppingToken);
            //await _bus.Publish(new QuartzMessage
            //{
            //    Destination = new Uri("queue:quartz"),
            //    Payload = new { Name = "Gabriel" },
            //    PayloadType = new string[1] { "Person" },
            //    Schedule = new EachMinute(),
            //}, stoppingToken);
        }

        private async Task SendEmailMessageAsync(CancellationToken cancellationToken)
        {
            AsyncServiceScope scope = default;

            try
            {
                scope = _serviceScopeFactory.CreateAsyncScope();

                var messageScheduler = scope.ServiceProvider.GetRequiredService<IMessageScheduler>();
                
                await messageScheduler.SchedulePublish(
                    scheduledTime: new DateTime(),
                    message: new SendEmailMessage(),
                    cancellationToken: cancellationToken);
            }
            finally
            {
                scope.Dispose();
            }
        }

        private async Task SendQuartzMessageAsync(CancellationToken cancellationToken)
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
                    scheduledTime: new DateTime(),
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
