namespace navsaar.api.ViewModels.Booking
{
    public class SendToDraftRequest
    {
        public int UserId { get; set; }
        public int BookingId { get; set; }
        public string? Notes { get; set; }
        public bool IsOriginalAgreement { get; set; }
    }
}
