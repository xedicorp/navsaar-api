using System.ComponentModel.DataAnnotations.Schema;

namespace navsaar.api.Models
{
    [Table("tblPlotChangeLogs")]
    public class PlotChangeLog
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public int NewPlotId { get; set; }
        public decimal NewAgreementValue { get; set; }
        public int OldPlotId { get; set; }
        public decimal OldAgreementValue { get; set; }
        public int PlotChangedBy { get; set; }
        public int PlotChangedOn { get; set; }
    }
}
