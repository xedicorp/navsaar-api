using System.ComponentModel.DataAnnotations.Schema;

namespace navsaar.api.Models
{
    [Table("tblDraftRequests")]
    public class DraftRequest
    {
        public int Id { get; set; }       
        public int BookingId { get; set; }
        public string? Notes { get; set; }
        public bool IsOriginalAgreement { get; set; }
        public int RequestedBy { get; set; }
        public DateTime RequestedDate { get; set; }
        public int Status { get; set; }
    }
}
