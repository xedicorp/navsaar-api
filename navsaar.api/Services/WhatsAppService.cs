using DocumentFormat.OpenXml.InkML;
using navsaar.api.Infrastructure;
using navsaar.api.Models;
using navsaar.api.Repositories;
using navsaar.api.ViewModels;
using Newtonsoft.Json;
using Org.BouncyCastle.Bcpg.OpenPgp;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace navsaar.api.Services
{
    public class WhatsAppService : IWhatsAppService
    {
        string accountSid = "";// Environment.GetEnvironmentVariable("TWILIO_ACCOUNT_SID");
        string authToken = ""; // Environment.GetEnvironmentVariable("TWILIO_AUTH_TOKEN");
        string contentSID = "";
        string from = "";
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        public WhatsAppService(  IConfiguration configuration, AppDbContext context)
        {
            _context = context;
            _configuration = configuration;
            accountSid = _configuration["Twilio:AccountSID"];
            authToken = _configuration["Twilio:AuthToken"];
            from = _configuration["Twilio:FromPhoneNumber"];
        }

        public async void SendMessage(BookingUpdate update, string plotNo, 
            string associateName, string associateNo)
        {
            try
            {

            
                TwilioClient.Init(accountSid, authToken);
                switch (update)
                {
                    case BookingUpdate.Hold:
                        contentSID = "HX596d37a6a4133a3a875e2ae63dc4a27f";
                        break;
                }
                var contentVariables = new Dictionary<string, string>
                {
                    { "name",associateName },
                    { "townshipname", "Navsaar Valley"},
                    {"plotno", plotNo   }
                };

                await MessageResource.CreateAsync(
                        from: new Twilio.Types.PhoneNumber("whatsapp:" + from),
                        to: new Twilio.Types.PhoneNumber("whatsapp:" + associateNo),
                        contentSid: contentSID,
                        contentVariables: Newtonsoft.Json.JsonConvert.SerializeObject(contentVariables)
                );
            }
            catch (Exception ex)
            { 

            }
        }
        public void SendMessage(
            BookingUpdate update, Booking booking)
        {
            try
            {
                var associate = _context.Associates.FirstOrDefault(p => p.ID == booking.AssociateId);
                var plot = _context.Plots.FirstOrDefault(p => p.Id == booking.PlotId);
                TwilioClient.Init(accountSid, authToken);
                string message = string.Empty;



                switch (update)
                {
                    case BookingUpdate.New:
                        contentSID = "HX86a7d543985de4f48c746ea53c2aa681";
                        if (!string.IsNullOrEmpty(booking.ClientContactNo))
                        {
                            //Send To client

                            this.Send1(booking.ClientContactNo, booking, message);
                        }
                        //Send To Associate
                        if (associate != null)
                        {
                            contentSID = "HX04220119c9bc44d94d55557ab8e7656f";
                            this.Send(booking.AssociateContactNo, booking, message);
                        }
                        //Send To Leader
                        if (associate != null && !string.IsNullOrEmpty(associate.LeaderContactNo))
                        {
                            contentSID = "HX6ac4292c575225c7819496dd9733a286";
                            this.Send3(associate.LeaderContactNo, associate.FirstName + " " + associate.LastName ?? "",associate.LeaderName  , plot.PlotNo);
                        }
                        break;
                    case BookingUpdate.InititalPaymentUpdate:

                        if (associate != null)
                        {
                            contentSID = "HX86a7d543985de4f48c746ea53c2aa681";

                            this.Send2(booking.AssociateContactNo, booking, message, associate.FirstName +" " +associate.LastName??"");
                        }
                        break;
                    case BookingUpdate.BookingAmountReceived:
                        contentSID = "HXe019195b1d2cae126dca65e4b9536a98";
                        break;
                    case BookingUpdate.LoanDocumentReminder:
                        contentSID = "HX5a28d901cd1e5ed23eac027c197fb1df";
                        break;
                    case BookingUpdate.JDAPattaReminder:
                        contentSID = "HXc2f0c977ff09e66a0a074dba3b973fcf";
                        break;
                    case BookingUpdate.JDAPattaApplied:
                        contentSID = "HX596d37a6a4133a3a875e2ae63dc4a27f";
                        break;
                   
                   
                    case BookingUpdate.Cancelled:
                        contentSID = "HXc778749b2004ea49beaea5ea02984e08";
                        break;

                  

                    case BookingUpdate.RefundInitiated:
                        contentSID = "HX098262b271e69247a15b3ceba3d2a674";
                        break;

                }


               
 
            }
            catch (Exception ex)
            {


            }
        }
        public void SendInitialPaymentStatusVerifyUpdate(
          BookingUpdate update, Booking booking, int status)
        {
            try
            {
                var associate = _context.Associates.FirstOrDefault(p => p.ID == booking.AssociateId);
                var plot = _context.Plots.FirstOrDefault(p => p.Id == booking.PlotId);
                TwilioClient.Init(accountSid, authToken);
                string message = string.Empty;



                if (!string.IsNullOrEmpty(booking.ClientContactNo) && status == 1)
                {
                    var contentVariables = new Dictionary<string, string>
                {
                    { "customername", booking.ClientName },
                    { "plotno", plot.PlotNo },
                     { "projectname", "Navsaar Valley" }
                };
                    contentSID = "HX69b9541a68d99304734bf31257915e50";
                    SendGeneric(booking.ClientContactNo, contentVariables);
                }
                if (!string.IsNullOrEmpty(associate.ContactNo) && status == 1)
                {
                    var contentVariables = new Dictionary<string, string>
                    {
                            { "associatename", associate.FirstName + " " + associate.LastName ?? "" },
                            { "customername", booking.ClientName },
                            { "plotno", plot.PlotNo },
                            { "projectname", "Navsaar Valley" }
                    };
                    contentSID = "HX42badfa1deea131d2106be31c3a54ae3";
                    SendGeneric(associate.ContactNo, contentVariables);
                }
                if (!string.IsNullOrEmpty(associate.LeaderContactNo) && status == 1)
                {
                    var contentVariables = new Dictionary<string, string>
                    {
                            { "leadername", associate.LeaderName   },
                            { "customername", booking.ClientName },
                            { "plotno", plot.PlotNo },
                            { "projectname", "Navsaar Valley" }
                    };
                    contentSID = "HX78e7cddb9733f9b5399e4cf925ffa638";
                    SendGeneric(associate.LeaderContactNo, contentVariables);
                }

            }
            catch (Exception ex)
            {


            }
        }

        private async void SendGeneric(string to,  Dictionary<string, string> contentVariables)
        {
          
            await MessageResource.CreateAsync(
                    from: new Twilio.Types.PhoneNumber("whatsapp:" + from),
                    to: new Twilio.Types.PhoneNumber("whatsapp:" + to),
                    contentSid: contentSID,
                    contentVariables: Newtonsoft.Json.JsonConvert.SerializeObject(contentVariables)
            );
        }
        private async void Send(string to, Booking update, string message)
        {
                var contentVariables = new Dictionary<string, string>
                {
                    { "1", update.ClientName },
                    { "2", update.PlotNo }
                };

                await MessageResource.CreateAsync(
                        from: new Twilio.Types.PhoneNumber("whatsapp:" + from),
                        to: new Twilio.Types.PhoneNumber("whatsapp:" + to),
                        contentSid: contentSID,
                        contentVariables: Newtonsoft.Json.JsonConvert.SerializeObject(contentVariables)
                );
        }
        private async void Send1(string to, Booking update, string message)
        {
            var contentVariables = new Dictionary<string, string>
                {
                    { "clientname", update.ClientName },
                    { "plotno", update.PlotNo }
                };

            await MessageResource.CreateAsync(
                    from: new Twilio.Types.PhoneNumber("whatsapp:" + from),
                    to: new Twilio.Types.PhoneNumber("whatsapp:" + to),
                    contentSid: contentSID,
                    contentVariables: Newtonsoft.Json.JsonConvert.SerializeObject(contentVariables)
            );
        }
        private async void Send2(string to, Booking update, string message,string associateName)
        {
            
            var contentVariables = new Dictionary<string, string>
                {
                    { "projectname", "Navsaar Valley"},
                    { "associatename", associateName },
                    { "customername", update.ClientName },
                    { "plotno", update.PlotNo }
                };

            await MessageResource.CreateAsync(
                    from: new Twilio.Types.PhoneNumber("whatsapp:" + from),
                    to: new Twilio.Types.PhoneNumber("whatsapp:" + to),
                    contentSid: contentSID,
                    contentVariables: Newtonsoft.Json.JsonConvert.SerializeObject(contentVariables)
            );
        }
        private async void Send3(string to,  string associateName, string leadername, string plotNo)
        {

            var contentVariables = new Dictionary<string, string>
                {
                    { "leadername",leadername},                  
                    { "associatename", associateName },                 
                    { "plotno", plotNo }
                };

            await MessageResource.CreateAsync(
                    from: new Twilio.Types.PhoneNumber("whatsapp:" + from),
                    to: new Twilio.Types.PhoneNumber("whatsapp:" + to),
                    contentSid: contentSID,
                    contentVariables: Newtonsoft.Json.JsonConvert.SerializeObject(contentVariables)
            );
        }
    }
}
