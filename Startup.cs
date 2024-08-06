using MassTransit;
using Quartz;
using TaskScheduler.Background;
using TaskScheduler.Consumers;

namespace TaskScheduler
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.Configure<RabbitMqTransportOptions>(Configuration.GetSection("RabbitMqTransport"));

            services.AddQuartz(quartz =>
            {
                quartz.SchedulerName = "Scheduler";
                quartz.SchedulerId = "Auto";

                quartz.UseMicrosoftDependencyInjectionJobFactory();
                quartz.UseDefaultThreadPool(threadPool => threadPool.MaxConcurrency = 10);
                quartz.UseTimeZoneConverter();
            });

            services.AddMassTransit(builder =>
            {
                Uri schedulerEndpoint = new Uri("queue:quartz");

                builder.AddMessageScheduler(schedulerEndpoint);
                builder.AddPublishMessageScheduler();
                builder.AddQuartzConsumers();
                builder.AddConsumer<MessageConsumer>();
                builder.UsingRabbitMq((context, cfg) =>
                {
                    cfg.UsePublishMessageScheduler();
                    cfg.ConfigureEndpoints(context);
                });
            });

            services.Configure<MassTransitHostOptions>(options =>
            {
                options.WaitUntilStarted = true;
            });

            services.AddQuartzHostedService(options =>
            {
                options.StartDelay = TimeSpan.FromSeconds(5);
                options.WaitForJobsToComplete = true;
            });

            services.AddHostedService<MessageTriggerBackgroundService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseRouting();
        }
    }
}
