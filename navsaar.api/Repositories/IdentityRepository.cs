

using Microsoft.EntityFrameworkCore;
using navsaar.api.Infrastructure;
using navsaar.api.Models;
using navsaar.api.ViewModels;
using navsaar.api.ViewModels.Identity;

namespace navsaar.api.Repositories
{
    public class IdentityRepository : IIdentityRepository
    {
        private readonly AppDbContext _context;
        public IdentityRepository(AppDbContext context)
        {
            _context = context;
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
        public LoginResponse Login(LoginRequest request)
        {
            List<string> q = new List<string>();
            bool isValid = _context.Users.Any(p => p.UserName == request.UserName && p.Password == request.Password && p.IsActive);
            if (!isValid)
            {
                return new LoginResponse
                {
                    IsSuccessful = false,
                    User = null,
                    Permissions = null
                };
            }
            var usr = _context.Users.FirstOrDefault(p => p.UserName == request.UserName 
            && p.Password == request.Password && p.IsActive);
          
            var userInfo = new UserInfo
            {
                Id = usr.Id,
                UserName = usr.UserName,
                IsActive = usr.IsActive,
                RoleId = usr.RoleId,
                 RoleName = _context.Roles.FirstOrDefault(r => r.Id == usr.RoleId)?.Name
            };
            if (usr.RoleId == 1)
            {
                _context.Permissions.Select(p => p.Name).ToList().ForEach(p => q.Add(p));
            }
            else
            {
                q = (from u in _context.Users
                     join r in _context.RolePermissions on u.RoleId equals r.RoleId
                     join p in _context.Permissions on r.PermissionId equals p.Id
                     where u.Id == usr.Id
                     select p.Name
                       ).ToList();
            }

            return new LoginResponse
            {
                IsSuccessful = true,
                User = userInfo,
                Permissions = q.ToList()
            };

        } 
    }
}
