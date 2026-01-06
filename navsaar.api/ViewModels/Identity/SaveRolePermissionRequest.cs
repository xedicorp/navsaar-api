namespace navsaar.api.ViewModels.Identity
{
    public class SaveRolePermissionRequest
    {
        public int RoleId { get; set; }
        public List<PermissionModel> Permissions { get; set; }
    }
}
