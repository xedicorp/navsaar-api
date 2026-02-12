namespace navsaar.api.ViewModels
{
    public class UpdateLoginStatusRequest
    {
        public int BookingId { get; set; }
        public DateTime DateOfLogin { get; set; }    
        public string BankName { get; set; } 
        public string BranchName { get; set; }
        public string LoginRefNo { get; set; }
        public string Notes { get; set; }
    }
    public class DraftPerparationStatusRequest
    {
        public int BookingId { get; set; }
        public bool IsDraftPrepared { get; set; }
        public DateTime? DraftPreparedOn { get; set; }
        public bool IsDraftGivenToBank { get; set; }
        public DateTime? DraftGivenToBankOn { get; set; }
        public string Notes { get; set; }
    }
    public class UploadDocumentRequest
    {
        public int BookingId { get; set; }
        public int DocumentTypeId { get; set; }    
        public IFormFile File { get; set; }
        public string Notes { get; set; }
        public bool IsDraft { get; set; }
        public bool IsATT { get; set; }
        public bool IsAllotment { get; set; }
    }
    public class  DocumentModel
    {
        public int DocumentId { get; set; }
        public int? BookingId { get; set; }
        public int DocumentTypeId { get; set; }
        public string? DocumentTypeName { get; set; }
        public string? FileName { get; set; }
        public string? Url { get; set; }
        public DateTime? UploadedOn { get; set; }
        public string? Notes { get; set; }
        public string? UploadedBy { get; set; }
        public bool IsDraft { get; set; }
        public bool IsATT { get; set; }
        public bool IsAllotment { get; set; }
    }
}
