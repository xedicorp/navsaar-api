namespace navsaar.api.ViewModels
{
    public class BookingInfo
    {
        public int  Id { get; set; }
        public string TownshipName { get; set; }
        public string PlotNo { get; set; }
        public decimal PlotSize { get; set; }
       public int PlotId { get; set; }  
        public decimal AgreementValue { get; set; }

        public DateTime BookingDate { get; set; }
        public string ClientName { get; set; }
        public string ClientEmail { get; set; }
        public string ContactNo { get; set; }

        public string AssociateName { get; set; }
        public string AssociateReraNo { get; set; }
        public string AssociateContactNo { get; set; }
        public string LeaderName { get; set; }

      
        public int TownshipId { get; set; }
     
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
        public DateTime? LoanSanctionDate { get; set; }
        public string? LoanSanctionNotes { get; set; }

        //Stateg6
        public bool? IsCompletedOnAllSides { get; set; }
        public DateTime? CompletionDate { get; set; }
        public string? MarkFileCheckNotes { get; set; }

        public string? OriginalATTPath { get; set; }
        public string? OriginalATTNotes { get; set; }

        public bool? IsDokitSigned { get; set; }
        public DateTime? DokitSignDate { get; set; }
        public bool? IsJDAFileSigned { get; set; }
        public DateTime? JDAFileSignDate { get; set; }
        public string? DokitSigingNotes { get; set; }

        public string? BankDDPath { get; set; }
        public string? DDNo { get; set; }
        public decimal? DDAmount { get; set; }
        public string? DDNotes { get; set; }
        //JDA Patta
        public bool? IsJDAPattaApplied { get; set; }
        public DateTime? JDAPattaAppliedOn { get; set; }
        public bool? IsJDAPattaRegistered { get; set; }
        public DateTime? JDAPattaRegisteredOn { get; set; }
        public bool? IsJDAPattaGivenToBank { get; set; }
        public DateTime? JDAPattaGivenToBankOn { get; set; }
        public bool? IsDDReceivedFromBank { get; set; }
        public DateTime? DDReceivedFromBankOn { get; set; }
        public string? JDAPattaNotes { get; set; }


        public bool? IsDDSubmittedToBank { get; set; }
        public DateTime? DDClearedOn { get; set; }
        public string? DDUpdateNotes { get; set; }
        public string? Status { get; set; }
    }
}
