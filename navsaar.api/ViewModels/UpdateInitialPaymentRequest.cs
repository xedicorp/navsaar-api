namespace navsaar.api.ViewModels
{
    public class UpdateInitialPaymentRequest
    {
        public int BookingId { get; set; }
        public int PaymentMode { get; set; }
        public decimal Amount { get; set; }
        public string TransNo { get; set; }
        public DateTime DateOfTransfer { get; set; }
        public bool IsPaymentVerified { get; set; }
        public string Notes { get; set; }
    }
}
