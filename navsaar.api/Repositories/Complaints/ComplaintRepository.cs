

using navsaar.api.Infrastructure;
using navsaar.api.Models;
using navsaar.api.ViewModels;
 

namespace navsaar.api.Repositories
{
    public class ComplaintRepository : IComplaintRepository
    {
        private readonly AppDbContext _context;
        public ComplaintRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<ComplaintInfo> List()
        {
            var q = from p in _context.Complaints
                    select new ComplaintInfo
                    {
                        ComplaintTypeId = p.ComplaintTypeId,
                        Id = p.Id,
                        ImagePath = p.ImagePath,
                        Notes = p.Notes,
                        SentBy = p.SentBy,
                        SentOn = p.SentOn,
                        Status = p.Status,
                    };
            return q.ToList();
        }
    }
}
