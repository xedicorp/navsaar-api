using System.ComponentModel.DataAnnotations.Schema;

namespace navsaar.api.ViewModels.Booking
{
  
    public class ChangePlotRequest
    {
        public int BookingId { get; set; }  
        public int NewPlotId { get; set; }
        public int NewAgreementValue { get; set; }
        public int PlotChangedBy { get; set; }
        public int PlotChangedOn { get; set; }
    }
}
