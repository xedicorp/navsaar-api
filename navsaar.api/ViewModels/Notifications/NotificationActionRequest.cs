namespace navsaar.api.ViewModels.Notifications
{
    public class NotificationActionRequest
    {
        public int NotificationId { get; set; }
        public int ActionType { get; set; } // 1 = Action Not Required, 2 = Action Required
    }
}