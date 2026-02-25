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

        public List<NotificationCount> CountByPriority()
        {
            return _context.Notifications
                .GroupBy(n => n.Priority)
                .Select(g => new NotificationCount
                {
                    Priority = g.Key,
                    PriorityName = g.Key == 1 ? "Normal" :
                                   g.Key == 2 ? "High" :
                                   g.Key == 3 ? "Critical" : "Unknown",
                    Count = g.Count()
                })
                .OrderBy(x => x.Priority)
                .ToList();
        }
    }
}
