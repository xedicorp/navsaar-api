using System.ComponentModel.DataAnnotations.Schema;

namespace navsaar.api.Models
{
    [Table("tblRefundStatusLogs")]
    public class RefundStatusLog
    {
        public int Id { get; set; } 
        public int RefundRequestId { get; set; }
        public int NewStatus { get; set; }
        public decimal Amount { get; set; }
        public DateTime ActualDate { get; set; }
        public DateTime StatusChangeDate { get; set; }
        public string? Notes { get; set; }
        public int StatusChangedBy { get; set; }
    }
}
