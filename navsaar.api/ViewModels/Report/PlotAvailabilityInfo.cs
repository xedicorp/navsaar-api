namespace navsaar.api.ViewModels.Report
{
    public class PlotAvailabilityInfo
    {
        public int Id { get; set; }
        public int TownshipId { get; set; }
        public string TownshipName { get; set; }

        public string PlotNo { get; set; }
        public decimal PlotSize { get; set; }
        public decimal? SaleableSize { get; set; }
        public decimal? PlotSizeInSqrmtr { get; set; }

        public int PlotTypeId { get; set; }
        public string PlotTypeName { get; set; }

        public string Status { get; set; }
    }
}