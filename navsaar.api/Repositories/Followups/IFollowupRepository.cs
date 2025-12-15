using navsaar.api.ViewModels.Followup;

namespace navsaar.api.Repositories.Followups
{
    public interface IFollowupRepository
    {
        List<FollowupInfo> ListByBookingId(int bookingId);
        List<FollowupInfo> GetByDay(string day);
        FollowupInfo GetById(int followupId);
        int Save(CreateEditFollowupRequest request);
    }
}
