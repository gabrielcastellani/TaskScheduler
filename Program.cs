using Quartz;
using TaskScheduler;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddQuartz(builder =>
{
    var jobKey = new JobKey("HelloJob");
    builder.AddJob<HelloJob>(options => options.WithIdentity(jobKey));
    builder.AddTrigger(
        options => options
            .ForJob(jobKey)
            .WithIdentity("HelloJob-Trigger")
            .WithCronSchedule("0 * * ? * *"));
});

services.AddQuartzHostedService(builder =>
{
    builder.WaitForJobsToComplete = false;
});

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();