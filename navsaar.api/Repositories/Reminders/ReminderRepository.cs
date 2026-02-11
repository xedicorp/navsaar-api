using navsaar.api.Infrastructure;
using navsaar.api.Models;
using navsaar.api.ViewModels.Reminders;

namespace navsaar.api.Repositories.Reminders
{
    public class ReminderRepository: IReminderRepository
    {
        private readonly AppDbContext _context;
        public ReminderRepository(AppDbContext context)
        {
            _context = context;
        }

        public bool Dismiss(int reminderId)
        {
            _context.Reminders.Find(reminderId).IsCompleted = true;
            _context.SaveChanges();
            return true;
        }

        public List<Reminder> List(int userId, bool IsPending = true)
        {
            return(from r in _context.Reminders
                   where r.CreatedBy == userId && r.IsCompleted != IsPending
                   select r).ToList();  
        }

        public int Save(CreateEditReminderRequest request)
        {
            var entity = new Reminder
            {
                ReminderDate = request.ReminderDate,
                ReminderTime = request.ReminderTime,
                Notes = request.Notes,
                IsCompleted = false,
                CreatedBy = request.CreatedBy,
                CreatedOn = DateTime.UtcNow,
                 BookingId=request.BookingId
            };
            _context.Reminders.Add(entity);
            _context.SaveChanges();
            return entity.Id;
        }
        public List<Reminder> GetByBookingId(int bookingId, bool isPending = true)
        {
            return _context.Reminders
                .Where(r => r.BookingId == bookingId
                         && r.IsCompleted != isPending)
                .ToList();
        }

    }
}
