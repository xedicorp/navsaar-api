namespace navsaar.api.ViewModels
{
    public class UpdateLoanSanctionStatusRequest
    {
        public int  BookingId { get; set; }
        public bool? IsLoanSanctioned { get; set; }
        public DateTime? LoanSanctionDate { get; set; }
        public string? Notes { get; set; }
    }
    public class UpdateMarkFileCheckStatusRequest
    {
        public int BookingId { get; set; }
        public bool? IsCompletedOnAllSides { get; set; }
        public DateTime? CompletionDate { get; set; }
        public string? Notes { get; set; }
    }
}
