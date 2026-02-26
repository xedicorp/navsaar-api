using navsaar.api.Models;

namespace navsaar.api.Repositories 
{
    public interface IBookingLogRepository
    {
        void Log(BookingLog log);
    }
}
