using navsaar.api.Models;

namespace navsaar.api.Repositories 
{
    public interface INotificationRepository
    {
        List<Notification> List(int priority);
    }
}
