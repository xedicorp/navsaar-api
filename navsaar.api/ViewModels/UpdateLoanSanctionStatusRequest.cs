namespace navsaar.api.ViewModels
{
    public class UpdateLoanSanctionStatusRequest
    {
        public int  BookingId { get; set; }
        public bool? IsLoanSanctioned { get; set; }
        public DateOnly? LoanSanctionDate { get; set; }
        public string? Notes { get; set; }
    }
}
