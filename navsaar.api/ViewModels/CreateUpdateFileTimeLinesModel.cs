namespace navsaar.api.ViewModels.FileTimelines
{
    public class CreateUpdateFileTimelinesModel
    {
        public int Id { get; set; }
        public int? StatusId { get; set; }
        public int Days { get; set; }
        public int? WorkflowTypeId { get; set; }
    }
}
