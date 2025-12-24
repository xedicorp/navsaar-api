namespace navsaar.api.ViewModels.Report
{
    public class TownshipCollectionDetail
    {
        public string TownshipName { get; set; }
        public int BookingNo { get; set; }
        public string CustomerName { get; set; }
        public string CustomerContactNo { get; set; }
        public string Description { get; set; } 
        public DateTime ReceiptDate { get; set; }
        public decimal Amount { get; set; }
    }
}
