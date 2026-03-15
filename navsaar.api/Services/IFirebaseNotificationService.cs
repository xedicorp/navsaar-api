namespace navsaar.api.Services
{
    public interface IFirebaseNotificationService
    {
       void Send( string fcmToken, string otp);
    }
}
