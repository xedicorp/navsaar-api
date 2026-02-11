using navsaar.api.Models;
using navsaar.api.ViewModels.Reminders;

namespace navsaar.api.Repositories.Reminders
{
    public interface IReminderRepository
    {
        List<Reminder> List(int userId, bool IsPending=true);
     
        int Save(CreateEditReminderRequest request);
        bool Dismiss(int reminderId);
        List<Reminder> GetByBookingId(int bookingId, bool isPending = true);

    }
}
