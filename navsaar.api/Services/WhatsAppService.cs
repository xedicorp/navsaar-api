using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace navsaar.api.Services
{
    public class WhatsAppService
    {
        string accountSid = "ACxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";
        string authToken = "your_auth_token";

        public void SendWhatsAppMessage()
        {
            TwilioClient.Init(accountSid, authToken);

            var message = MessageResource.Create(
                from: new PhoneNumber("whatsapp:+919414553440"), // Twilio WhatsApp number
                to: new PhoneNumber("whatsapp:+918955036762"),    // Recipient number
                body: "Hello from .NET via Twilio WhatsApp 👋"
            );
        }
    }
}
