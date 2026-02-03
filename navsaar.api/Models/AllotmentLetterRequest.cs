using System.ComponentModel.DataAnnotations.Schema;

namespace navsaar.api.Models
{
    [Table("tblAllotmentLetterRequests")]
    public class AllotmentLetterRequest
    {
        public int Id { get; set; }       
        public int BookingId { get; set; }
        public string? Notes { get; set; } 
        public int RequestedBy { get; set; }
        public DateTime RequestedDate { get; set; }
        public int Status { get; set; }
    }
}
