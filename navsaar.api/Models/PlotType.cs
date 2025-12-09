using System.ComponentModel.DataAnnotations.Schema;

namespace navsaar.api.Models
{
    [Table("tblPlotTypes")]
    public class PlotType
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
