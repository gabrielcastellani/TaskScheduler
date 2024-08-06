using Quartz;

namespace TaskScheduler
{
    internal sealed class HelloJob : IJob
    {
        private readonly ILogger<HelloJob> _logger;

        public HelloJob(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<HelloJob>();
        }

        public Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("Hello world!");
            return Task.CompletedTask;
        }
    }
}
