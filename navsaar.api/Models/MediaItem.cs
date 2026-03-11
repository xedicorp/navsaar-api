using System.ComponentModel.DataAnnotations.Schema;

namespace navsaar.api.Models
{
    [Table("tblMediaItems")]
    public class MediaItem
    {
        public int Id { get; set; }
        public string? Title { get; set; }  
        public string? Description { get; set; }  
        public int? MediaTypeId { get; set; } 
        public string? MediaUrl { get; set; }
        public string? MediaThumnailPath { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}