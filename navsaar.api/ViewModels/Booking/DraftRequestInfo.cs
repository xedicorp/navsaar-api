namespace navsaar.api.ViewModels.Booking
{
    public class DraftRequestInfo
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
      
        public int? Status { get; set; }
        public string? Notes { get; set; }

        public DateTime RequestedOn { get; set; }
        public string RequestedByName { get; set; }

        public int BookingId { get; set; }
        public string PlotNo { get; set; }
        public string CustomerName { get; set; }
    }
}
