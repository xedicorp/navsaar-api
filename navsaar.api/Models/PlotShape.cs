using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace navsaar.api.Models
{
    [Table("tblPlotShape")]
    public class PlotShape
    {
        [Key]
        public int Id { get; set; }

        [StringLength(100)]
        public string? ShapeName { get; set; }
    }
}
