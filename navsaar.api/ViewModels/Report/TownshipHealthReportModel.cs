namespace navsaar.api.ViewModels.Report
{
    public class TownshipHealthReportModel
    {
        public string TownshipName { get; set; }
        public string TownshipAddress { get; set; }
        public int TotalPlotsCount { get; set; }
        public int TotalBookedPlotsCount { get; set; }
        public int TotalUnbookedPlotsCount { get; set; }

        public decimal TotalArea{ get; set; }
        public decimal TotalBookedArea { get; set; }
        public decimal TotalUnbookedArea { get; set; }

     
        public decimal TotalBookedValue { get; set; }
        public decimal TotalAmountReceived { get; set; }
        public decimal  TotalAmountPending { get; set; }
        public decimal  TotalAmountRefunded { get; set; } 
    }
}
