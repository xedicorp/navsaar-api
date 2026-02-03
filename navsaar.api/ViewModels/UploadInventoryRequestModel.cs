namespace navsaar.api.ViewModels
{
    public class UploadInventoryRequestModel
    {
        public int UserId { get; set; }
        public int TownshipId { get; set; }
        public IFormFile? File { get; set; }
    }
}
