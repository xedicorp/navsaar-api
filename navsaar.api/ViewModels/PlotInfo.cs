namespace navsaar.api.ViewModels
{
    public class PlotInfo
    {
        public int Id { get; set; }
        public int TownshipId { get; set; }
        public string TownshipName { get; set; }
        public string PlotNo { get; set; }
        public decimal  PlotSize { get; set; }
        public decimal? SaleableSize{ get; set; }
        public decimal? PlotSizeInSqrmtr { get; set; }
        public int Facing { get; set; }
        public string FacingName { get; set; }
        public bool IsCorner { get; set; }
        public bool IsTPoint { get; set; }
        public bool IsTapper { get; set; }
        public int PlotTypeId { get; set; }
        public string PlotTypeName { get; set; }
        public string Status { get; set; }
        public decimal? RoadSize { get; set; }
        public decimal? PLC { get; set; }   

    }
    public class PlotHoldRequestModel
    {
        public int PlotId { get; set; }
        public int AssociateId { get; set; }

        public int? WorkflowTypeId { get; set; }
        public int? TownshipId { get; set; }
        public decimal? PlotSize { get; set; }
        public decimal? AgreementRate { get; set; }
        public decimal? TotalAgreementValue { get; set; }
    }

}
