using System.ComponentModel.DataAnnotations.Schema;

namespace navsaar.api.Models
{
    [Table("tblBookingCancelLogs")]
    public class BookingCancelLog
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public string CancelReason { get; set; }
        public int CancelledBy { get; set; }
        public DateTime CancelledOn { get; set; }
    }
}
