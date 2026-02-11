using navsaar.api.Infrastructure;
using navsaar.api.ViewModels.Associate;

namespace navsaar.api.Repositories
{
    public class AssociateRepository : IAssociateRepository
    {
        private readonly AppDbContext _context;

        public AssociateRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<AssociateInfo> GetList()
        {
            return _context.Associates
                .Select(a => new AssociateInfo
                {
                    Id = a.ID,
                    UserName = a.UserName,
                    FirstName = a.FirstName,
                    ContactNo = a.ContactNo,     // can be NULL
                    LeaderName = a.LeaderName,   // can be NULL
                    ReraNo = a.RERA,
                    IsActive = a.IsActive,
                    IsApproved = a.IsApproved
                })
                .ToList();
        }
        public AssociateInfo GetByRera(string reraNo)
        {
            return _context.Associates
                .Where(a => a.RERA == reraNo)
                .Select(a => new AssociateInfo
                {
                    Id = a.ID,
                    UserName = a.UserName,
                    FirstName = a.FirstName,
                    ContactNo = a.ContactNo,
                    LeaderName = a.LeaderName,
                    ReraNo = a.RERA,
                    IsActive = a.IsActive,
                    IsApproved = a.IsApproved
                })
                .FirstOrDefault();
        }
    }
}
