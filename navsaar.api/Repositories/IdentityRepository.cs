

using Microsoft.EntityFrameworkCore;
using navsaar.api.Infrastructure;
using navsaar.api.Models;
using navsaar.api.Repositories.Identity;
using navsaar.api.ViewModels;
using navsaar.api.ViewModels.Identity;

namespace navsaar.api.Repositories
{
    public class IdentityRepository : IIdentityRepository
    {
        private readonly AppDbContext _context;
        private readonly IPermissionRepository _permissionRepository;
        public IdentityRepository(AppDbContext context, IPermissionRepository permissionRepository)
        {
            _context = context;
            _permissionRepository = permissionRepository;
        }
        public List<UserInfo> List()
        {
             return( from p in _context.Users
                     join r in _context.Roles on p.RoleId equals r.Id   
                     select new UserInfo
                    {
                        Id = p.Id,
                        UserName = p.UserName,
                        IsActive = p.IsActive,
                         RoleId = p.RoleId,
                         RoleName = r.Name  
                     }).ToList();

        }
        public List<RoleInfo> RoleList()
        {
            return (from p in _context.Roles
                    
                    select new RoleInfo
                    {
                        Id = p.Id,
                        Name = p.Name
                    }).ToList();

        }
        public LoginResponse Login(LoginRequest request)
        {
          
            bool isValid = _context.Users.Any(p => p.UserName == request.UserName && p.Password == request.Password && p.IsActive);
            if (isValid)
            {
                var usr = _context.Users.FirstOrDefault(p => p.UserName == request.UserName
                        && p.Password == request.Password && p.IsActive);

                List<PermissionInfo> permissions = _permissionRepository.GetRolePermissions(usr.RoleId);
                string roleName = _context.Roles.FirstOrDefault(r => r.Id == usr.RoleId)?.Name;

                return new LoginResponse
                {
                    IsSuccessful = true,
                    User = new UserInfo
                    {
                        Id = usr.Id,
                        IsActive = usr.IsActive,
                        RoleId = usr.RoleId,
                        RoleName = roleName,
                        UserName = usr.UserName
                    },
                    Permissions = permissions
                };
            }
            else
            {
                return new LoginResponse
                {
                    IsSuccessful = false,
                    User = null,
                    Permissions = null
                };
            }
        }

        public List<PermissionInfo> RolePermissions(int roleId)
        {
            return _permissionRepository.GetRolePermissions(roleId);

        }

        public List<UserTownshipInfo> UserTownships(int userId)
        {
            return _permissionRepository.GetUserTownships (userId);
        }

        public bool AssignTownships(AssignUserTownshipsRequest request)
        {
            return _permissionRepository.AssignTownships(request.UserId,request.UserTownships );
        }
    }
}
