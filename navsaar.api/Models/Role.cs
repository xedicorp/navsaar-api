using System.ComponentModel.DataAnnotations.Schema;

namespace navsaar.api.Models
{
    [Table("tblRoles")]
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
