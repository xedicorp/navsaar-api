namespace navsaar.api.Models
{
    public class FirebaseMessage
    {
        public Message message { get; set; }
    }
    public class Message
    {
        public string token { get; set; }
        public FCMNotification notification { get; set; }
        public Dictionary<string, string>? data { get; set; }
    }

    public class FCMNotification
    {
        public string title { get; set; }
        public string body { get; set; }
    }
}
