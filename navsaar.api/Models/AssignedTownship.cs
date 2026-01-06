using System.ComponentModel.DataAnnotations.Schema;

namespace navsaar.api.Models
{
    [Table("tblAssignedTownships")]
    public class AssignedTownship
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TownshipId { get; set; }
    }
}
