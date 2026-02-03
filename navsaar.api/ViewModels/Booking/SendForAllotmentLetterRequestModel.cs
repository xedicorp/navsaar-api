namespace navsaar.api.ViewModels 
{
    public class SendForAllotmentLetterRequestModel
    {
        public int UserId { get; set; }
        public int BookingId { get; set; }
        public string? Notes { get; set; }
        public bool IsOriginalAgreement { get; set; }
    }
}
