using System.ComponentModel.DataAnnotations.Schema;

namespace navsaar.api.Models
{
    [Table("tblTownships")]
    public class Township
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
