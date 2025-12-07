using System.ComponentModel.DataAnnotations.Schema;

namespace navsaar.api.Models
{
    [Table("tblBookings")]
    public class Booking
    {
        public int Id { get; set; }
        public DateTime BookingDate { get; set; }
        public int TownshipId { get; set; }
        public string? PlotNo { get; set; }
        public string? PlotSize { get; set; }
        public string? ClientName { get; set; }
        public string? ClientContactNo { get; set; }
        public string ClientEmail { get; set; }
        public string? AssociateName { get; set; }
        public string AssociateReraNo { get; set; }
        public string? AssociateContactNo { get; set; }
        public string? LeaderName { get; set; }
        public string? ChequeFilePath { get; set; }
        public int? WorkflowTypeId { get; set; }
        public int? CurrentStage { get; set; }
        public int? PaymentMode { get; set; }
        public decimal? Amount_2 { get; set; }
        public string? TransNo { get; set; }
        public DateTime? DateOfTransfer { get; set; }
        public bool? IsPaymentVerified { get; set; }
        public string? Notes_2 { get; set; }

        //Stage3
        public DateTime? DateOfLogin { get; set; }
        public string? BankName { get; set; }
        public string? BranchName { get; set; }
        public string? LoginRefNo { get; set; }
        public string? Notes_3 { get; set; }

        //stage4
        public bool? IsDraftPrepared { get; set; }
        public DateTime? DraftPreparedOn { get; set; }
        public bool? IsDraftGivenToBank { get; set; }
        public DateTime? DraftGivenToBankOn { get; set; }
        public string? Notes_4 { get; set; }

        //stage5
        public bool? IsLoanSanctioned { get; set; }
        public DateOnly? LoanSanctionDate { get; set; }
        public string? LoanSanctionNotes { get; set; }

        
    }
}