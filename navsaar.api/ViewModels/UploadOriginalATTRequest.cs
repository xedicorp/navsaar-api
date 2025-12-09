namespace navsaar.api.ViewModels
{
    public class UploadOriginalATTRequest
    {
        public int BookingId { get; set; }
        public IFormFile File { get; set; }
        public string  Notes { get; set; }
    }
}
