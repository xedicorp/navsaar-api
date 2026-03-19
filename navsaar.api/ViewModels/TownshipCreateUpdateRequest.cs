namespace navsaar.api.ViewModels
{
    public class TownshipCreateUpdateRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string? RERAno { get; set; }
    }
}
