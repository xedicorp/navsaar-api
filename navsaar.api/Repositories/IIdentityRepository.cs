

using navsaar.api.ViewModels;
using navsaar.api.ViewModels.Identity;

namespace navsaar.api.Repositories
{
    public interface IIdentityRepository
    {
        List<UserInfo> List();
        LoginResponse Login(LoginRequest request);
        List<RoleInfo> RoleList();
        List<PermissionInfo> RolePermissions(int roleId);
        bool SaveRolePermissions(SaveRolePermissionRequest request);
        List<UserTownshipInfo> UserTownships(int userId);
        bool AssignTownships(AssignUserTownshipsRequest request);
    }
}
