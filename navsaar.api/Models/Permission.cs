using System.ComponentModel.DataAnnotations.Schema;

namespace navsaar.api.Models
{
    [Table("tblPermissions")]
    public class Permission
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
