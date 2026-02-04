using navsaar.api.Repositories;
using Newtonsoft.Json;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace navsaar.api.Services
{
    public class WhatsAppService : IWhatsAppService
    {
        string accountSid = "";// Environment.GetEnvironmentVariable("TWILIO_ACCOUNT_SID");
        string authToken = ""; // Environment.GetEnvironmentVariable("TWILIO_AUTH_TOKEN");

        IBookingRepository _bookingRepository;
        private readonly IConfiguration _configuration;
        public WhatsAppService(IBookingRepository bookingRepository, IConfiguration configuration)
        {
            _bookingRepository = bookingRepository;
            _configuration = configuration;
        }


        public void SendMessage(  
            BookingUpdate update, int bookingId)
        {
           
            string message = string.Empty;
            var booking = _bookingRepository.GetById(bookingId);
          

            switch(update)
            {
                case BookingUpdate.New:
                        message= string.Format( "Mr./Ms. {1}, your booking has been confirmed, for Plot No {0}", 
                            booking.PlotNo, booking.ClientName);
                    break;

            }

            TwilioClient.Init(accountSid, authToken);
            //Send To client
            this.Send(booking.ContactNo,  update, message);
            //Send To Associate
            this.Send(booking.AssociateContactNo, update, message);

            // var message = await MessageResource.CreateAsync(
            //from: new Twilio.Types.PhoneNumber("whatsapp:+14155238886"),
            //to: new Twilio.Types.PhoneNumber("whatsapp:+919414553440"),
            //contentSid: "HXb5b62575e6e4ff6129ad7c8efe1f983e",
            //contentVariables: JsonConvert.SerializeObject(
            //    new Dictionary<string, Object>() { { "1", "22 July 2026" }, { "2", "3:15pm" } },
            //    Formatting.Indented));
        }


        private void Send(string to,   BookingUpdate update, string message)
        {
          
           // string from = "9982022122";
           string from= "+14155238886";
            var msg = MessageResource.Create(
                from: new PhoneNumber("whatsapp:"+from), // Twilio WhatsApp number
                to: new PhoneNumber("whatsapp:+91"+to),    // Recipient number
                body: message
            ); 
        } 
    }
}
