using navsaar.api.Models;

namespace navsaar.api.Repositories.Notifications
{
    public interface INotificationRepository
    {
        bool ExistsForToday(int bookingId, string type);
        void Add(Notification notification);
    }
}