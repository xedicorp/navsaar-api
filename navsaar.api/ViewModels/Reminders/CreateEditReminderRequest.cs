namespace navsaar.api.ViewModels.Reminders
{
    public class CreateEditReminderRequest
    {
        public int Id { get; set; }
        public int? BookingId { get; set; }
        public DateOnly ReminderDate { get; set; }
        public TimeOnly ReminderTime { get; set; }
        
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public string Notes { get; set; }
    }
}
