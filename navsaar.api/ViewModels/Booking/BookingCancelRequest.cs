namespace navsaar.api.ViewModels.Booking
{
    public class BookingCancelRequest
    {
        public int BookingId { get; set; }
        public string CancelReason { get; set; }
        public int CancelledBy { get; set; }
        public DateTime CancelledOn { get; set; }
    }
}
