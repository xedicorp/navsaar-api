namespace navsaar.api.ViewModels.Refund
{
    public class SaveRefundStatusModel
    {
        public int RefundRequestId { get; set; } 
        public int NewStatus { get; set; }
        public decimal Amount { get; set; }
        public DateTime ActualDate { get; set; }
        public string? Notes { get; set; }
        public int UserId { get; set; }
    }
}
