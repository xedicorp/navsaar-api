namespace navsaar.api.ViewModels.Associate
{
    public class CreateUpdateAssociateModel
    {
        public long Id { get; set; }   // 0 = Create, >0 = Update

        public string? FirstName { get; set; }
        public string? ReraNo { get; set; }
        public string? ContactNo { get; set; }
        public string? LeaderName { get; set; }
    }
}
