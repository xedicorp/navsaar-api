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

        
        private readonly IConfiguration _configuration;
        public WhatsAppService(  IConfiguration configuration)
        {
           
            _configuration = configuration;
            accountSid = _configuration["Twilio:AccountSID"];
            authToken = _configuration["Twilio:AuthToken"];
        }


        public void SendMessage(  
            BookingUpdate update, Booking  booking)
        {
           
            string message = string.Empty;
            
          

            switch(update)
            {
                case BookingUpdate.New:
                        message= string.Format( "Mr./Ms. {1}, your booking has been confirmed, for Plot No {0}", 
                            booking.PlotNo, booking.ClientName);
                    break;

                case BookingUpdate.BankLoginDone:
                    message =
                        $"Mr./Ms. {booking.ClientName},\n" +
                        $"Your bank login process has been completed successfully.\n\n" +
                        $"Plot No: {booking.PlotNo}\n" +
                        $"Bank: {booking.BankName}\n" +
                        $"Login Reference No: {booking.LoginRefNo}\n\n" +
                        $"– Navsaar Group";
                    break;

                case BookingUpdate.BankDDReceived:
                    message =
                        $"Mr./Ms. {booking.ClientName},\n" +
                        $"Your Bank Demand Draft (DD) has been received successfully.\n\n" +
                        $"Plot No: {booking.PlotNo}\n" +
                        $"DD Cleared On: {booking.DDClearedOn:dd MMM yyyy}\n\n" +
                        $"– Navsaar Group";
                    break;

                case BookingUpdate.SentForJDAPatta:
                    message =
                        $"Mr./Ms. {booking.ClientName},\n" +
                        $"Your plot documents have been sent for JDA Patta processing.\n\n" +
                        $"Plot No: {booking.PlotNo}\n" +
                        $"Applied On: {booking.JDAPattaAppliedOn:dd MMM yyyy}\n\n" +
                        $"– Navsaar Group";
                    break;

                case BookingUpdate.LoanSanctioned:
                    message =
                        $"Mr./Ms. {booking.ClientName},\n" +
                        $"We are pleased to inform you that your loan has been sanctioned successfully.\n\n" +
                        $"Plot No: {booking.PlotNo}\n" +
                        $"Sanction Date: {booking.LoanSanctionDate:dd MMM yyyy}\n\n" +
                        $"– Navsaar Group";
                    break;

                case BookingUpdate.SentToAllotmentLetter:
                    message =
                        $"Mr./Ms. {booking.ClientName},\n" +
                        $"Your request for Allotment Letter has been initiated successfully.\n\n" +
                        $"Plot No: {booking.PlotNo}\n" +
                        $"Status: Under preparation\n\n" +
                        $"– Navsaar Group";
                    break;

                case BookingUpdate.AllotmentLetterReceived:
                    message =
                        $"Mr./Ms. {booking.ClientName},\n" +
                        $"Your Allotment Letter has been prepared and received successfully.\n\n" +
                        $"Plot No: {booking.PlotNo}\n\n" +
                        $"– Navsaar Group";
                    break;

                case BookingUpdate.SentToDraft:
                    message =
                        $"Mr./Ms. {booking.ClientName},\n" +
                        $"Your documents have been sent for Draft Agreement preparation.\n\n" +
                        $"Plot No: {booking.PlotNo}\n" +
                        $"Status: Under preparation\n\n" +
                        $"– Navsaar Group";
                    break;

                case BookingUpdate.DokitSigned:
                    message =
                        $"Mr./Ms. {booking.ClientName},\n" +
                        $"Your Dokit and related documents have been signed successfully.\n\n" +
                        $"Plot No: {booking.PlotNo}\n" +
                        $"Signed On: {booking.DokitSignDate:dd MMM yyyy}\n\n" +
                        $"– Navsaar Group";
                    break;

                case BookingUpdate.Cancelled:
                    message =
                        $"Mr./Ms. {booking.ClientName},\n" +
                        $"We regret to inform you that your booking has been cancelled.\n\n" +
                        $"Plot No: {booking.PlotNo}\n\n" +
                        $"If you have made any payments, the refund process will be initiated as applicable.\n\n" +
                        $"– Navsaar Group";
                    break;

                case BookingUpdate.PaymentReceived:
                    message =
                        $"Mr./Ms. {booking.ClientName},\n" +
                        $"We have received your payment details.\n\n" +
                        $"Plot No: {booking.PlotNo}\n" +
                        $"Amount: ₹{booking.Amount_2}\n" +
                        $"Transaction No: {booking.TransNo}\n\n" +
                        $"– Navsaar Group";
                    break;

                case BookingUpdate.PaymentConfirmed:
                    message =
                        $"Mr./Ms. {booking.ClientName},\n" +
                        $"Your payment has been verified and confirmed successfully.\n\n" +
                        $"Plot No: {booking.PlotNo}\n" +
                        $"Amount: ₹{booking.Amount_2}\n\n" +
                        $"– Navsaar Group";
                    break;

                case BookingUpdate.RefundInitiated:
                    message =
                        $"Mr./Ms. {booking.ClientName},\n" +
                        $"Your refund process has been initiated successfully.\n\n" +
                        $"Plot No: {booking.PlotNo}\n\n" +
                        $"The amount will be credited to your account as per bank processing timelines.\n\n" +
                        $"– Navsaar Group";
                    break;

            }


            TwilioClient.Init(accountSid, authToken);
            //Send To client
            this.Send(booking.ClientContactNo,  update, message);
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
