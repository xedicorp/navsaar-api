using navsaar.api.Infrastructure;
using navsaar.api.Models;
using navsaar.api.ViewModels.Followup;

namespace navsaar.api.Repositories.Followups
{
    public class FollowupRepository : IFollowupRepository
    {
        private readonly AppDbContext _context;
        public FollowupRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<FollowupInfo> GetByDay(string day)
        {
            DateTime today = DateTime.Today; 

            return (
                       (from p in _context.Followups
                        join ft in _context.FollowupTypes on p.FollowupTypeId equals ft.Id
                        join u in _context.Users on p.CreatedBy equals u.Id
                        where p.FollowupDate.Date == (day.ToLower() == "today" ? today :
                                                (day.ToLower() == "tomorrow" ? today.AddDays(1) :
                                                (day.ToLower() == "week" ? today.AddDays(6) : today)))
                        select new FollowupInfo
                        {
                            Id = p.Id,
                            BookingId = p.BookingId,
                            FollowupDate = p.FollowupDate,
                            Notes = p.Notes,
                            CreatedBy = u.UserName,
                            FollowupTypeName = ft.Name

                        }).ToList()

                );
        }

        public FollowupInfo GetById(int followupId)
        {
            throw new NotImplementedException();
        }

        public List<FollowupInfo> ListByBookingId(int bookingId)
        {
            return (
                       (from p in _context.Followups
                        join ft in _context.FollowupTypes on p.FollowupTypeId equals ft.Id
                        join u in _context.Users on p.CreatedBy equals u.Id
                        where p.BookingId == bookingId
                        select new FollowupInfo
                        {
                            Id = p.Id,
                            BookingId = p.BookingId,
                            FollowupDate = p.FollowupDate,
                            Notes = p.Notes,
                            CreatedBy = u.UserName,
                            FollowupTypeName = ft.Name

                        }).ToList()

                );
        }

        public int Save(CreateEditFollowupRequest request)
        {
            Followup entity = new Followup();
            if (request.Id != 0)
            {
                entity = _context.Followups.Find(request.Id);

            } 

            entity.BookingId = request.BookingId;           
            entity.CreatedBy = request.CreatedBy;
            entity.Notes = request.Notes;
            entity.FollowupTypeId = request.FollowupTypeId;
            entity.FollowupDate = DateTime.Now;

            if (request.Id == 0)
            {
                _context.Followups.Add(entity);
            }
            _context.SaveChanges();
            return entity.Id;
        }
    }
}
