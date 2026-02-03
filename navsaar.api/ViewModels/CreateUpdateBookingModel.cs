namespace navsaar.api.ViewModels
{
    public class CreateUpdateBookingModel
    {
        public int Id { get; set; }
        public DateTime BookingDate { get; set; }
        public int TownshipId { get; set; }
        public int PlotId { get; set; } 
        public string PlotNo { get; set; }
        public decimal AgreementValue { get; set; }
        public decimal PlotSize { get; set; }
        public string ClientName { get; set; }
        public string ClientContactNo { get; set; }
        public string? ClientAddress { get; set; }
        public string? ClientEmail { get; set; }
        public string AssociateName { get; set; }
        public string AssociateReraNo { get; set; }
        public string AssociateContactNo { get; set; }
        public string LeaderName { get; set; } 
        public int WorkflowTypeId { get; set; }
        public  IFormFile?  File { get; set; }
        public int DocumentTypeId { get; set; }
    }
}
