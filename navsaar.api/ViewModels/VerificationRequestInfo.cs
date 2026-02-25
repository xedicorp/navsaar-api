namespace navsaar.api.ViewModels
{
    public class VerificationRequestInfo
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime? ReceiptDate { get; set; }
        public int ReceiptMethod { get; set; }
        public string TransactionId { get; set; }
        public string? BankName { get; set; }
        public string? ChequeNo { get; set; }
        public int? Status { get; set; }
        public string? Notes { get; set; }

        public DateTime RequestedOn { get; set; }
        public string RequestedByName { get; set; }

        public int BookingId { get; set; }  
        public string PlotNo { get; set; }
        public string CustomerName { get; set; }
    }
    public class VerificationRequestApiResponse
    {
        public int PendingCount { get; set; }
        public List<VerificationRequestInfo> Data { get; set; } = new();
    }
}
