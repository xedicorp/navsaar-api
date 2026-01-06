using navsaar.api.ViewModels.Identity;

namespace navsaar.api.Repositories.Identity
{
    public interface IPermissionRepository
    {
        List<PermissionInfo> GetRolePermissions(int roleId=0);
        List<UserTownshipInfo> GetUserTownships(int userId);
      bool  AssignTownships(int userId, List<UserTownshipModel> userTownships);
        bool AssignRolePermissions(int roleId, List<PermissionModel> permissions);
    }
}
