using MailKit.Net.Smtp;
using MassTransit;
using MimeKit;
using TaskScheduler.Messages;

namespace TaskScheduler.Consumers
{
    internal class SendEmailMessageConsumer : IConsumer<SendEmailMessage>
    {
        private readonly ILogger<SendEmailMessageConsumer> _logger;

        public SendEmailMessageConsumer(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<SendEmailMessageConsumer>();
        }

        public Task Consume(ConsumeContext<SendEmailMessage> context)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("TaskScheduler - Service", context.Message.SmtpUsername));
            email.To.Add(new MailboxAddress("Gabriel Castellani", "email@gmail.com"));
            email.Subject = "Hello World";
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = "This is a test email sent using C#",
            };

            try
            {
                using (var smtp = new SmtpClient())
                {
                    smtp.Connect(context.Message.SmtpHost, context.Message.SmtpPort, false);
                    smtp.Authenticate(context.Message.SmtpUsername, context.Message.SmtpPassword);
                    smtp.Send(email);
                    smtp.Disconnect(true);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao enviar email");
            }

            return Task.CompletedTask;
        }
    }
}
