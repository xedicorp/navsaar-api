using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace navsaar.api.Models
{
    [Table("tblCloserRequest")]
    public class CloserRequest
    {
        [Key]
        public int Id { get; set; }

        public int BookingId { get; set; }

        public int UserId { get; set; }

        public DateTime Date { get; set; }

        public int Status { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}