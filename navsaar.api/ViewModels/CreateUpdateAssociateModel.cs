namespace navsaar.api.ViewModels.Associate
{
    public class CreateUpdateAssociateModel
    {
        public long Id { get; set; }

        public string? FirstName { get; set; }
        public DateTime? DOB { get; set; }
        public DateTime? AnniversaryDate { get; set; }
        public string? ContactNo { get; set; }

        public string? PANNo { get; set; }
        public string? AadhaarNo { get; set; }

        public string? ReraNo { get; set; }
        public IFormFile? ReraCertificateFile { get; set; }

        public IFormFile? PhotoFile { get; set; }

        public string? PassportNo { get; set; }
        public IFormFile? PassportFile { get; set; }

        public IFormFile? BankDocumentFile { get; set; }

        public string? LeaderName { get; set; }
        public string? LeaderContactNo { get; set; }
    }
}
