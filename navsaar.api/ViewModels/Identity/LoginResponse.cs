namespace navsaar.api.ViewModels.Identity
{
    public class LoginResponse
    {
        public string? Token { get; set; }
        public UserInfo? User { get; set; }
        public List<PermissionInfo>? Permissions { get; set; }
        public bool IsSuccessful { get; set; }
    }
}
