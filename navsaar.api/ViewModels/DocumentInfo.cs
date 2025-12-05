namespace navsaar.api.ViewModels
{
    public class DocumentInfo
    {
        public int Id { get; set; }
        public int? BookingId { get; set; }       
        public string? DocumentTypName { get; set; }
        public string? FilePath { get; set; }
        public DateTime UploadedOn { get; set; }
        public string UploadedBy { get; set; }
        public string? Notes { get; set; }
    }
}
