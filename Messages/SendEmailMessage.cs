namespace TaskScheduler.Messages
{
    public class SendEmailMessage
    {
        public string SmtpHost { get; set; } = "smtp.mailersend.net";
        public int SmtpPort { get; set; } = 587;
        public string SmtpUsername { get; set; }
        public string SmtpPassword { get; set; }
    }
}
