using navsaar.api.Infrastructure;
using navsaar.api.Models;

namespace navsaar.api.Repositories.Notifications
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly AppDbContext _context;

        public NotificationRepository(AppDbContext context)
        {
            _context = context;
        }

        public bool ExistsForToday(int bookingId, string type)
        {
            return _context.Notifications.Any(n =>
                n.BookingId == bookingId &&
                n.NotificationType == type &&
                n.CreatedOn.Date == DateTime.Today
            );
        }

        public void Add(Notification notification)
        {
            _context.Notifications.Add(notification);
            _context.SaveChanges();
        }
    }
}