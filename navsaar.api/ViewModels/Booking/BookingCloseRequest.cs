using System.ComponentModel.DataAnnotations.Schema;

namespace navsaar.api.Models
{
    public class SendForCloserRequest
    {
        public int BookingId { get; set; }
        public int UserId { get; set; }
    }
    public class CloseBookingRequest
    {
        public int CloserRequestId { get; set; }
        public int UserId { get; set; }
    }
    public class ApproveRejectClosureRequestModel 
    {
        public int CloserRequestId { get; set; }
        public int UserId { get; set; }
        public bool ApproverResponse { get; set; }
        public string Notes { get; set; }
      
    }
    
    public class AddCloserRequestDetailRequest
    {
        public int CloserId { get; set; }
        public int UserId { get; set; }
        public string? Reason { get; set; }
    }
    public class CloserRequestInfo
    {
        public int Id { get; set; }

        public int BookingId { get; set; }

        public string CustomerName { get; set; }

        public string PlotNo { get; set; }

        public string TownshipName { get; set; }

        public decimal? Amount { get; set; }

        public string RequestedBy { get; set; }

        public DateTime RequestedOn { get; set; }

        public int Status { get; set; }
    }
}
