namespace TaskScheduler.Messages
{
    public class Message
    {
        public string Value { get; set; }

        public Message(string value)
        {
            Value = value;
        }
    }
}
