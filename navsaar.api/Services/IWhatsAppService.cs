using navsaar.api.Models;
using navsaar.api.ViewModels;

namespace navsaar.api.Services
{
    public interface IWhatsAppService
    {
        void SendMessage(BookingUpdate update,   Booking  booking);
    } 
}
