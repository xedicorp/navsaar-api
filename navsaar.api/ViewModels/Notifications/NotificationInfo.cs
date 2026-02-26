namespace navsaar.api.ViewModels.Notifications
{
    public class NotificationInfo
    {
        public int Id { get; set; }
        public string NotificationText { get; set; } = null!;
        public int BookingId { get; set; }
        public string NotificationType { get; set; } = null!;
        public string Priority { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsRead { get; set; }
    }
}
