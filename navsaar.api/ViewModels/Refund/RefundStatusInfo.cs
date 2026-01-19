namespace navsaar.api.ViewModels.Refund
{
    public class RefundStatusInfo
    {
     
        public DateTime StatusChangedOn { get; set; }
        public string StatusChangedBy{ get; set; }
        public string Status { get; set; }
        public decimal Amount { get; set; }
        public DateTime ActualDate { get; set; }
        public string? Notes { get; set; }
    
    }
}
