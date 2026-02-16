using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace navsaar.api.Models
{
    [Table("tblPlotHoldRequest")]
    public class PlotHoldRequest
    {
        [Key]
        public int Id { get; set; }

        public int PlotId { get; set; }

        public int AssociateId { get; set; }

        public DateTime HoldDateTime { get; set; }

        public bool IsDelete { get; set; }
    }
}
