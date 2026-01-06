

using Microsoft.EntityFrameworkCore;
using navsaar.api.Infrastructure;
using navsaar.api.ViewModels;

namespace navsaar.api.Repositories
{
    public class TownshipRepository : ITownshipRepository
    {
        private readonly AppDbContext _context;
        public TownshipRepository(AppDbContext context)
        {
            _context = context;
        }
        public List<TownshipInfo> List()
        {
             return( from p in _context.Townships
                    select new TownshipInfo
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Address= p.Address
                    }).ToList();

        }

        public bool Save(TownshipCreateUpdateRequest request)
        {
            var entity = new Models.Township();
            entity.Name = request.Name;
            entity.Address = request.Address;
            if (request.Id>0)
            {
                entity = _context.Townships.Find(request.Id);
                if (entity == null)
                {
                    return false;
                }
                entity.Name  = request.Name;
                entity.Address=request.Address;
            }
            if (request.Id == 0)
                _context.Townships.Add(entity);

            _context.SaveChanges();
            return true;

        }
    }
}
