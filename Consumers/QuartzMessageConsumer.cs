using MassTransit;
using TaskScheduler.Messages;

namespace TaskScheduler.Consumers
{
    public class QuartzMessageConsumer : IConsumer<QuartzMessage>
    {
        private readonly ILogger<QuartzMessageConsumer> _logger;

        public QuartzMessageConsumer(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<QuartzMessageConsumer>();
        }

        public Task Consume(ConsumeContext<QuartzMessage> context)
        {
            _logger.LogInformation("Received Quartz message.");
            return Task.CompletedTask;
        }
    }
}
