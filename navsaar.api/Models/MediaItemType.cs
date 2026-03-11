using System.ComponentModel.DataAnnotations.Schema;

namespace navsaar.api.Models
{
    [Table("tblMediaItemTypes")]
    public class MediaItemType
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}