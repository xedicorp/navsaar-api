using navsaar.api.Infrastructure;
using navsaar.api.Models;

namespace navsaar.api.Repositories 
{
    public class BookingLogRepository : IBookingLogRepository
    {
        private readonly AppDbContext _context;

        public BookingLogRepository(AppDbContext context)
        {
            _context = context;
        }
        public void Log(BookingLog log)
        {
            _context.BookingLogs.Add(log);
            _context.SaveChanges(); 
        }
    }
}
