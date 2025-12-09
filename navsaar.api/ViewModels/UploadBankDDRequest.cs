namespace navsaar.api.ViewModels
{
    public class UploadBankDDRequest
    {
        public int BookingId { get; set; }
        public IFormFile File { get; set; }
        public string? DDNo { get; set; }
        public decimal? DDAmount { get; set; }
        public string? DDNotes { get; set; }
    }
}
