using System.ComponentModel.DataAnnotations.Schema;

namespace navsaar.api.Models
{
    [Table("tblFollowupTypes")]
    public class FollowupType
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
