namespace navsaar.api.ViewModels
{
    public class CreateUpdateComplaintModel
    {
        public int Id { get; set; }
        public IFormFile? Image { get; set; }
        public int SentBy { get; set; }
        public string? Notes { get; set; }
        public int ComplaintTypeId { get; set; }
        public int TownshipId { get; set; }
    }
}