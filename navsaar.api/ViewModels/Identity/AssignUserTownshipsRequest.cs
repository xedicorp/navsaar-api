namespace navsaar.api.ViewModels.Identity
{
    public class AssignUserTownshipsRequest
    {
        public int UserId { get; set; } 
        public List<UserTownshipModel> UserTownships { get; set; }
    }
}
