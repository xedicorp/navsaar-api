using navsaar.api.Models;
using navsaar.api.ViewModels.Notifications;

namespace navsaar.api.Repositories 
{
    public interface INotificationRepository
    {
        List<NotificationInfo> List(int priority, int userId);
    }
}
