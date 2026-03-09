using System.ComponentModel.DataAnnotations.Schema;

namespace navsaar.api.Models
{
    [Table("tblComplaintType")]
    public class ComplaintType
    {
        public int Id { get; set; }
        public string? Name { get; set; }

    }
}
