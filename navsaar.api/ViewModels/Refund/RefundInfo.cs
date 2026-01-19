namespace navsaar.api.ViewModels.Refund
{
    public class RefundInfo
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public string Status { get; set; } //1:Pending 2:Approved 3:Rejected 4:Processed 5:Completed
        public decimal RefundAmount { get; set; }
        public string Notes { get; set; }
        public BookingInfo BookingInfo { get; set; }
    }
}
