using MassTransit;
using TaskScheduler.Messages;

namespace TaskScheduler.Consumers
{
    internal sealed class MessageConsumer : IConsumer<Message>
    {
        private readonly ILogger<MessageConsumer> _logger;

        public MessageConsumer(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<MessageConsumer>();
        }

        public Task Consume(ConsumeContext<Message> context)
        {
            _logger.LogInformation("Received scheduled message: {value}", context.Message.Value);
            return Task.CompletedTask;
        }
    }
}
