namespace navsaar.api.ViewModels.FileTimelines
{
    public class FileTimelinesInfo
    {
        public int Id { get; set; }
        public int? StatusId { get; set; }
        public string? StatusName { get; set; }
        public int Days { get; set; }
        public int? WorkflowTypeId { get; set; }
    }
}
