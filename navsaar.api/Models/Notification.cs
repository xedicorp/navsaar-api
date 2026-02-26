using System.ComponentModel.DataAnnotations.Schema;
namespace navsaar.api.Models
{
    [Table("tblNotifications")]
    public class Notification
    {
        public int Id { get; set; }
        public string NotificationText { get; set; } = null!;
        public int BookingId { get; set; }
        public string NotificationType { get; set; } = null!;
        public int Priority { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsRead { get; set; }
    }
    public class NotificationCount
    {
        public int Priority { get; set; }
        public string PriorityName { get; set; } = null!;

        public int Count { get; set; }
    }
}