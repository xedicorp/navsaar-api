using System.ComponentModel.DataAnnotations.Schema;

namespace navsaar.api.Models
{
    [Table("tblReminders")]
    public class Reminder
    {
        public int Id { get; set; }
        public int? BookingId { get; set; }
        public DateOnly  ReminderDate { get; set; }
        public TimeOnly ReminderTime { get; set; }
        public bool IsCompleted { get; set; }   
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public string Notes { get; set; }       
    }
}
