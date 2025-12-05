

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
                        Name = p.Name
                    }).ToList();

        }
    }
}
