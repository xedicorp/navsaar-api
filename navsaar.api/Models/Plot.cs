using System.ComponentModel.DataAnnotations.Schema;

namespace navsaar.api.Models
{
    [Table("tblPlots")]
    public class Plot
    {
        public int Id { get; set; }
        public int TownshipId { get; set; }
        public string PlotNo { get; set; }
        public decimal PlotSize { get; set; }
        public int Facing { get; set; }
        public bool IsCorner { get; set; }
        public bool IsTPoint { get; set; }
        public bool IsTapper { get; set; }
        public int PlotTypeId { get; set; }
        public int? Status { get; set; }
        public decimal? SaleableSize { get; set; }
        public decimal? PlotSizeInSqrmtr { get; set; }
    }
}
