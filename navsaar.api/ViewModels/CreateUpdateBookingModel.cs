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
        public decimal? TotalAgreementValue { get; set; }
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
        public string? LeaderContactNo { get; set; }
        public string? RelationType { get; set; }
        public string? RelationName { get; set; }



        // Initial payment fields
        public decimal? InitialAmount { get; set; }
        public DateTime? InitialPaymentDate { get; set; }
        public int? InitialReceiptMethod { get; set; }
        public string? InitialTransactionId { get; set; }
        public string? InitialBankName { get; set; }
        public string? InitialChequeNo { get; set; }
        public string? InitialNotes { get; set; }
        public IFormFile? InitialReceiptImage { get; set; }
        public int? InitialReceiptStatus { get; set; }

    }
}
