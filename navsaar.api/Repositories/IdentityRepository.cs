

using Microsoft.EntityFrameworkCore;
using navsaar.api.Infrastructure;
using navsaar.api.ViewModels;

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

        //public bool Save(TownshipCreateUpdateRequest request)
        //{
        //    var entity = new Models.Township();
        //    entity.Name = request.Name;
        //    if (request.Id>0)
        //    {
        //        entity = _context.Townships.Find(request.Id);
        //        if (entity == null)
        //        {
        //            return false;
        //        }
        //        entity.Name  = request.Name;
        //    }
        //    if (request.Id == 0)
        //        _context.Townships.Add(entity);

        //    _context.SaveChanges();
        //    return true;

        //}
    }
}
