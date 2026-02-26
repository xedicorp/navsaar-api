using System.ComponentModel.DataAnnotations.Schema;

namespace navsaar.api.Models
{
    [Table("tblBookingLogs")]
    public class BookingLog
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public DateTime Timestamp { get; set; }
        public string Message { get; set; }
        public int UserId { get; set; }
    }
}
