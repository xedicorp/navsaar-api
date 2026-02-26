using navsaar.api.Infrastructure;
using navsaar.api.Models;
using navsaar.api.ViewModels.Notifications;

namespace navsaar.api.Repositories
{
    public class NotificationRepository:INotificationRepository
    {
        private readonly AppDbContext _context;
        public NotificationRepository(AppDbContext context)
        {
            _context = context;
        }
        public List<NotificationInfo> List(int priority=0, int userId=0)
        {
             
            // Filter notifications based on the provided priority
            var q =  from p in _context.Notifications
                     where ( priority==0 || p.Priority == priority)
                     select new NotificationInfo
                     {
                         Id = p.Id,
                         BookingId = p.BookingId,
                         NotificationText = p.NotificationText,
                         NotificationType = p.NotificationType,
                         Priority = GetPriorityName( p.Priority),
                         CreatedOn = p.CreatedOn,
                         IsRead = p.IsRead
                     };
            return q.ToList();
        }
        private static string GetPriorityName(int priority)
        {
            switch (priority)
            {
                case 3:
                    return "High";
                case 2:
                    return "Medium";
                case 1:
                    return "Low";
                default:
                    return "Very Low";
            }
        }
    }
}
