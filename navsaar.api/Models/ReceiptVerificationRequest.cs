using System.ComponentModel.DataAnnotations.Schema;

namespace navsaar.api.Models
{
    [Table("tblReceiptVerificationRequests")]
    public class ReceiptVerificationRequest
    {
        public int Id { get; set; }
        public int ReceiptId { get; set; }
        public DateTime RequestedOn { get; set; }
        public int RequestedBy
        { get; set; }
        public int Status
        { get; set; }
    }
}