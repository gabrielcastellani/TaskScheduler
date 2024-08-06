using MassTransit;
using TaskScheduler.Messages;

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
            while (!stoppingToken.IsCancellationRequested)
            {
                AsyncServiceScope scope = default;

                try
                {
                    scope = _serviceScopeFactory.CreateAsyncScope();

                    //    var bus = scope.ServiceProvider.GetRequiredService<IBus>();
                    //    var schedulerEndpoint = await bus.GetSendEndpoint(new Uri("queue:quartz"));

                    //await schedulerEndpoint.ScheduleRecurringSend(
                    //    new Uri("queue:Message"), new MessageSchedule(), new Message("Hello World!"));

                    var messageScheduler = scope.ServiceProvider.GetRequiredService<IMessageScheduler>();

                    await messageScheduler.SchedulePublish(
                        scheduledTime: new DateTime() + TimeSpan.FromSeconds(15),
                        message: new Message("Hello World!"),
                        cancellationToken: stoppingToken);
                }
                finally
                {
                    scope.Dispose();
                }

                Thread.Sleep(TimeSpan.FromSeconds(30));
            }
        }
    }
}
