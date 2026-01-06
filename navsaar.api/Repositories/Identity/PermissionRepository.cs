using Microsoft.EntityFrameworkCore;
using navsaar.api.Infrastructure;
using navsaar.api.Models;
using navsaar.api.ViewModels.Identity;

namespace navsaar.api.Repositories.Identity
{
    public class PermissionRepository: IPermissionRepository
    {
        private readonly AppDbContext _context;
        public PermissionRepository(AppDbContext context)
        {
            _context = context;
        }

        public bool AssignRolePermissions(int roleId, List<PermissionModel> permissions)
        {
            List<RolePermission> rolePerms = _context.RolePermissions.Where(p => p.RoleId == roleId).ToList();
            if (rolePerms.Count > 0)
            {
                _context.RolePermissions.RemoveRange(rolePerms);
                _context.SaveChanges();
            }


            foreach (var permission in permissions)
            {
                if (permission.IsAssigned)
                {
                    RolePermission rolePermission = new RolePermission
                    {
                        RoleId = roleId,
                        PermissionId = permission.PermissionId 
                    };
                    _context.RolePermissions.Add(rolePermission);
                }
                _context.SaveChanges();
            }
            return true;
        }

        public bool AssignTownships(int userId, List<UserTownshipModel> userTownships)
        {
            List<UserTownship> townships = _context.UserTownships.Where(p => p.UserId == userId).ToList();
            if(townships.Count > 0)
            {
                _context.UserTownships.RemoveRange(townships);
                _context.SaveChanges();
            }
            

            foreach (var township in userTownships)
            {
                if (township.IsAssigned)
                {
                    UserTownship userTownship = new UserTownship
                    {
                        UserId = userId,
                        TownshipId = township.TownshipId
                    };
                    _context.UserTownships.Add(userTownship);
                }
                _context.SaveChanges();
            }
            return true;
        }

        public List<PermissionInfo> GetRolePermissions(int roleId=0)
        {
            List<PermissionInfo> permissionInfos = new List<PermissionInfo>();
           List<RolePermission> rolePermissions=  _context.RolePermissions.Where(p =>roleId==0 ||  p.RoleId == roleId).ToList();
            List<Permission> permissions = _context.Permissions.ToList();
            foreach (var permission in permissions)
            {
                bool isAssigned = rolePermissions.Any(rp => rp.PermissionId == permission.Id);
                if(roleId==1)
                    isAssigned = true;
                permissionInfos.Add(new PermissionInfo
                {
                    Id = permission.Id,
                     Name = permission.Name,
                     IsAssigned=isAssigned
                });
                 
            }
            return permissionInfos;
        }

        //public void UpdatePermissions(int roleId, List<int> permissions)
        //{
        //    var rolePerms = db.RolePrivileges.Where(p => p.RoleId == roleId).ToList();
        //    foreach (var rolePerm in rolePerms)
        //    {
        //        rolePerm.IsGranted = false;
        //        if (permissions != null && permissions.Contains(rolePerm.PrivilegeId))
        //        {
        //            rolePerm.IsGranted = true;
        //        }
        //    }

        //    db.SaveChanges();
        //}
        public List<UserTownshipInfo> GetUserTownships(int userId)
        {
            var user=  _context.Users.FirstOrDefault(p => p.Id == userId); 

            List<UserTownshipInfo> userTownshipInfos = new List<UserTownshipInfo>();
            List<UserTownship> userTownships = _context.UserTownships.Where(p => p.UserId==userId).ToList();
            List<Township> townships = _context.Townships.ToList();
            foreach (var township in townships)
            {
                bool isAssigned = userTownships.Any(rp => rp.TownshipId == township.Id);
                if (user.RoleId == 1)
                    isAssigned = true;
                userTownshipInfos.Add(new UserTownshipInfo
                {
                    TownshipId = township.Id,
                    TownshipName = township.Name,   
                    IsAssigned = isAssigned
                });

            }
            return userTownshipInfos;
        }
    }
}
