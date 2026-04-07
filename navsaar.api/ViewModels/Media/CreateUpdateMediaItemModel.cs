namespace navsaar.api.ViewModels.Media
{
    public class CreateUpdateMediaItemModel
    {
        public int Id { get; set; }  

        public string? Title { get; set; }

        public string? Description { get; set; }

        public int? MediaTypeId { get; set; }

        public string? MediaUrl { get; set; }

        public IFormFile? ThumbnailFile { get; set; }
        public int? TownshipId { get; set; }
        public IFormFile? VideoFile { get; set; }
    }
}