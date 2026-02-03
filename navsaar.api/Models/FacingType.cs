using System.ComponentModel.DataAnnotations.Schema;

namespace navsaar.api.Models
{
    [Table("tblFacings")]
    public class FacingType
    {
        public int Id { get; set; }
        public string FacingName { get; set; }
    }
}
