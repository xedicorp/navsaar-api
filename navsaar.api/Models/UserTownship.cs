using System.ComponentModel.DataAnnotations.Schema;
namespace navsaar.api.Models
{
    [Table("tblUserTownships")]
    public class UserTownship
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TownshipId { get; set; }
    }
}
