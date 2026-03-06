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
        public bool IsTransactional { get; set; }
        public bool IsActionTaken { get; set; }
        public int? ActionType { get; set; }   // 1 = Action Not Required, 2 = Action Required
        public DateTime? ActionOn { get; set; }

    }
    public class NotificationCount
    {
        public int Priority { get; set; }
        public string PriorityName { get; set; } = null!;

        public int Count { get; set; }
    }
}