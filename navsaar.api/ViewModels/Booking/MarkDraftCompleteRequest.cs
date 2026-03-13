namespace navsaar.api.ViewModels.Booking
{
    public class MarkDraftCompleteRequest
    {
        public int  Id { get; set; }
        public int UserId { get; set; }
        public IFormFile? File { get; set; }
        public string Notes { get; set; }
    }
}
