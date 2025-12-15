namespace navsaar.api.ViewModels.Followup
{
    public class FollowupInfo
    {
     
        public int Id { get; set; }
        public int BookingId { get; set; }
        public DateTime FollowupDate { get; set; }
        public string Notes { get; set; }
        public string CreatedBy { get; set; }
        public  string FollowupTypeName { get; set; }
    }
}
