using System.ComponentModel.DataAnnotations.Schema;

namespace navsaar.api.Models
{
    [Table("tblFileTimelines")]
    public class FileTimeline
    {
        public int Id { get; set; } 
        public int StatusId { get; set; }
        public int Days { get; set; } 
    }
}
