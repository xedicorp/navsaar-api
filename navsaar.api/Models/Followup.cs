using System.ComponentModel.DataAnnotations.Schema;

namespace navsaar.api.Models
{
    [Table("tblFollowups")]
    public class Followup
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public DateTime FollowupDate { get; set; }
        public string Notes { get; set; }
        public int CreatedBy { get; set; }
        public int FollowupTypeId { get; set; }
    }
}
