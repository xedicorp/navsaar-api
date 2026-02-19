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
        public string? ApplicantName { get; set; }
        public string? RelativeName { get; set; }
        public string? RelativeType { get; set; }

        public string? Address { get; set; }
        public string? ContactNo { get; set; }
    }
}
