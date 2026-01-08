using System.ComponentModel.DataAnnotations.Schema;

namespace navsaar.api.Models
{
    [Table("tblRefundRequests")]
    public class  RefundRequest 
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public int Status { get; set; } //1:Pending 2:Approved 3:Rejected 4:Processed 5:Completed
        public decimal RefundAmount { get; set; }
        public string Notes { get; set; }
        public DateTime LastStatusChangedOn { get; set; }
        public int LastStatusChangedBy { get; set; }
    }
}
