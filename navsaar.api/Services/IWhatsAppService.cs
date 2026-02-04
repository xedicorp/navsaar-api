namespace navsaar.api.Services
{
    public interface IWhatsAppService
    {
        void SendMessage(BookingUpdate update, int bookingId);
    } 
}
