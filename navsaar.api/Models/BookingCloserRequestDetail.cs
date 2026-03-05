using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace navsaar.api.Models
{
    [Table("tblCloserRequestDetail")]
    public class CloserRequestDetail
    {
        [Key]
        public int Id { get; set; }

        public int CloserId { get; set; }

        public int UserId { get; set; }

        public string? Reason { get; set; }

        public DateTime Date { get; set; }
    }
}