namespace navsaar.api.ViewModels.Media
{
    public class MediaItemInfo
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int? MediaTypeId { get; set; }
        public string? MediaTypeName { get; set; }
        public string? MediaUrl { get; set; }
        public string? MediaThumnailPath { get; set; }
        public string? VideoFilePath { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? TownshipId { get; set; }
        public string? TownshipName { get; set; }
    }
}
