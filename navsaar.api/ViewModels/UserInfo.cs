namespace navsaar.api.ViewModels
{
    public class UserInfo
    {
        public int Id { get; set; }
        public string UserName { get; set; } 
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public bool IsActive { get; set; }
    }
}
