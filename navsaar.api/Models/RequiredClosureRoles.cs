using System.ComponentModel.DataAnnotations.Schema;

namespace navsaar.api.Models
{
    [Table("tblRequiredClosureRoles")]
    public class RequiredClosureRole
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
    }
}