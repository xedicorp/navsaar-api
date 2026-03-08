namespace navsaar.api.ViewModels 
{
    public class ComplaintInfo
    {
        public int Id { get; set; }
        public string? ImagePath { get; set; }
        public int? SentBy { get; set; }
        public DateTime SentOn { get; set; }
        public string? Notes { get; set; }
        public int? ComplaintTypeId { get; set; }
        public int? Status { get; set; }
    }
}
