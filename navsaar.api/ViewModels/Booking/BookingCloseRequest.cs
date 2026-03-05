using System.ComponentModel.DataAnnotations.Schema;

namespace navsaar.api.Models
{
    public class SendForCloserRequest
    {
        public int BookingId { get; set; }
        public int UserId { get; set; }
    }
    public class CloseBookingRequest
    {
        public int CloserRequestId { get; set; }
        public int UserId { get; set; }
    }
    public class AddCloserRequestDetailRequest
    {
        public int CloserId { get; set; }
        public int UserId { get; set; }
        public string? Reason { get; set; }
    }
}
