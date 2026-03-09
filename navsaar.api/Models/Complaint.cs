using System.ComponentModel.DataAnnotations.Schema;

namespace navsaar.api.Models
{
    [Table("tblComplaints")]
    public class Complaint
    {
        public int Id { get; set; }
        public string? ImagePath { get; set; }
        public int? SentBy { get; set; }
        public DateTime SentOn { get; set; }          
        public string? Notes { get; set; }
        public int? ComplaintTypeId { get; set; }
        public int? Status { get; set; }
        public int TownshipId { get; set; }

    }
}
