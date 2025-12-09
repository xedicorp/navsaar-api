namespace navsaar.api.ViewModels
{
    public class UpdateBankDDStatusRequest
    {
        public int BookingId { get; set; }
        public bool? IsDDSubmittedToBank { get; set; }
        public DateTime? DDClearedOn { get; set; }
        public string? Notes { get; set; }
    }
}
