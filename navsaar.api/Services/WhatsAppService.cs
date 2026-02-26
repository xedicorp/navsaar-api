using navsaar.api.Models;
using navsaar.api.Repositories;
using navsaar.api.ViewModels;
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
        string contentSID = "";
        string from = "";

        private readonly IConfiguration _configuration;
        public WhatsAppService(  IConfiguration configuration)
        {
           
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


                string message = string.Empty;



                switch (update)
                {
                    case BookingUpdate.New:
                        contentSID = "HXa17e8ccbee579e8c050d3d87b6fac57a";
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
                   
                    //case BookingUpdate.BankLoginDone:
                    //    message =
                    //        $"Mr./Ms. {booking.ClientName},\n" +
                    //        $"Your bank login process has been completed successfully.\n\n" +
                    //        $"Plot No: {booking.PlotNo}\n" +
                    //        $"Bank: {booking.BankName}\n" +
                    //        $"Login Reference No: {booking.LoginRefNo}\n\n" +
                    //        $"– Navsaar Group";
                    //    break;

                    //case BookingUpdate.BankDDReceived:
                    //    message =
                    //        $"Mr./Ms. {booking.ClientName},\n" +
                    //        $"Your Bank Demand Draft (DD) has been received successfully.\n\n" +
                    //        $"Plot No: {booking.PlotNo}\n" +
                    //        $"DD Cleared On: {booking.DDClearedOn:dd MMM yyyy}\n\n" +
                    //        $"– Navsaar Group";
                    //    break;

                    //case BookingUpdate.SentForJDAPatta:
                    //    message =
                    //        $"Mr./Ms. {booking.ClientName},\n" +
                    //        $"Your plot documents have been sent for JDA Patta processing.\n\n" +
                    //        $"Plot No: {booking.PlotNo}\n" +
                    //        $"Applied On: {booking.JDAPattaAppliedOn:dd MMM yyyy}\n\n" +
                    //        $"– Navsaar Group";
                    //    break;

                    //case BookingUpdate.LoanSanctioned:
                    //    message =
                    //        $"Mr./Ms. {booking.ClientName},\n" +
                    //        $"We are pleased to inform you that your loan has been sanctioned successfully.\n\n" +
                    //        $"Plot No: {booking.PlotNo}\n" +
                    //        $"Sanction Date: {booking.LoanSanctionDate:dd MMM yyyy}\n\n" +
                    //        $"– Navsaar Group";
                    //    break;

                    //case BookingUpdate.SentToAllotmentLetter:
                    //    message =
                    //        $"Mr./Ms. {booking.ClientName},\n" +
                    //        $"Your request for Allotment Letter has been initiated successfully.\n\n" +
                    //        $"Plot No: {booking.PlotNo}\n" +
                    //        $"Status: Under preparation\n\n" +
                    //        $"– Navsaar Group";
                    //    break;

                    //case BookingUpdate.AllotmentLetterReceived:
                    //    message =
                    //        $"Mr./Ms. {booking.ClientName},\n" +
                    //        $"Your Allotment Letter has been prepared and received successfully.\n\n" +
                    //        $"Plot No: {booking.PlotNo}\n\n" +
                    //        $"– Navsaar Group";
                    //    break;

                    //case BookingUpdate.SentToDraft:
                    //    message =
                    //        $"Mr./Ms. {booking.ClientName},\n" +
                    //        $"Your documents have been sent for Draft Agreement preparation.\n\n" +
                    //        $"Plot No: {booking.PlotNo}\n" +
                    //        $"Status: Under preparation\n\n" +
                    //        $"– Navsaar Group";
                    //    break;

                    //case BookingUpdate.DokitSigned:
                    //    message =
                    //        $"Mr./Ms. {booking.ClientName},\n" +
                    //        $"Your Dokit and related documents have been signed successfully.\n\n" +
                    //        $"Plot No: {booking.PlotNo}\n" +
                    //        $"Signed On: {booking.DokitSignDate:dd MMM yyyy}\n\n" +
                    //        $"– Navsaar Group";
                    //    break;

                    case BookingUpdate.Cancelled:
                        contentSID = "HXc778749b2004ea49beaea5ea02984e08";
                        break;

                    //case BookingUpdate.PaymentReceived:
                    //    message =
                    //        $"Mr./Ms. {booking.ClientName},\n" +
                    //        $"We have received your payment details.\n\n" +
                    //        $"Plot No: {booking.PlotNo}\n" +
                    //        $"Amount: ₹{booking.Amount_2}\n" +
                    //        $"Transaction No: {booking.TransNo}\n\n" +
                    //        $"– Navsaar Group";
                    //    break;

                    //case BookingUpdate.PaymentConfirmed:
                    //    message =
                    //        $"Mr./Ms. {booking.ClientName},\n" +
                    //        $"Your payment has been verified and confirmed successfully.\n\n" +
                    //        $"Plot No: {booking.PlotNo}\n" +
                    //        $"Amount: ₹{booking.Amount_2}\n\n" +
                    //        $"– Navsaar Group";
                    //    break;

                    case BookingUpdate.RefundInitiated:
                        contentSID = "HX098262b271e69247a15b3ceba3d2a674";
                        break;

                }


                TwilioClient.Init(accountSid, authToken);
                //Send To client
                this.Send(booking.ClientContactNo, booking, message);
                //Send To Associate
                this.Send(booking.AssociateContactNo, booking, message);

                // var message = await MessageResource.CreateAsync(
                //from: new Twilio.Types.PhoneNumber("whatsapp:+14155238886"),
                //to: new Twilio.Types.PhoneNumber("whatsapp:+919414553440"),
                //contentSid: "HXb5b62575e6e4ff6129ad7c8efe1f983e",
                //contentVariables: JsonConvert.SerializeObject(
                //    new Dictionary<string, Object>() { { "1", "22 July 2026" }, { "2", "3:15pm" } },
                //    Formatting.Indented));

            }
            catch (Exception ex)
            {


            }
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
    }
}
