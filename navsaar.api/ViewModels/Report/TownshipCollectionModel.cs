namespace navsaar.api.ViewModels.Report
{
    public class TownshipCollectionModel
    {
        public int Id { get; set; }
        public string TownshipName { get; set; }
        public decimal TodaysCollection { get; set; }
        public decimal TotalCollection { get; set; }
    }
}
