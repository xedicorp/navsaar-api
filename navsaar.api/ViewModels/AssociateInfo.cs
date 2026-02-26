namespace navsaar.api.ViewModels.Associate
{
    public class AssociateInfo
    {
        public long Id { get; set; }
        public string? UserName { get; set; }
        public string? FirstName { get; set; }
        public string? ContactNo { get; set; }
        public string? LeaderName { get; set; }
        public string? LeaderContactNo { get; set; }
        public string? ReraNo { get; set; }

        public DateTime? DOB { get; set; }
        public DateTime? AnniversaryDate { get; set; }

        public string? PANCardNo { get; set; }
        public string? AadhaarNo { get; set; }
        public string? PassportNo { get; set; }

        public bool? IsActive { get; set; }
        public bool? IsApproved { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public string? RERACertificateFile { get; set; }
        public string? PhotoFile { get; set; }
        public string? PassportFile { get; set; }
        public string? BankDocumentFile { get; set; }
    }
}
