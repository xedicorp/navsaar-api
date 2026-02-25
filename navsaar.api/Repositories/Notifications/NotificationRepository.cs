using navsaar.api.Infrastructure;
using navsaar.api.Models;

namespace navsaar.api.Repositories
{
    public class NotificationRepository:INotificationRepository
    {
        private readonly AppDbContext _context;
        public NotificationRepository(AppDbContext context)
        {
            _context = context;
        }
        public List<Notification> List(int priority)
        {
             
            // Filter notifications based on the provided priority
            return _context.Notifications.Where(n => n.Priority == priority).ToList();
        }
    }
}
