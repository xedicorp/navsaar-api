using DocumentFormat.OpenXml.Wordprocessing;
using navsaar.api.Models;

namespace navsaar.api.Infrastructure
{
    public class SchedulerService : ISchedulerService
    {
        private readonly AppDbContext _context;

        //STATUS → NOTIFICATION MESSAGE
       

        public SchedulerService(AppDbContext context)
        {
            _context = context;
        }

        public void GenerateNofications()
        {
            //First clear all notifications of today to avoid duplicates (if any)
            var today = DateTime.Today;
            List<Notification> todaysNotifications = new List<Notification>();
            foreach (var booking in _context.Bookings.ToList())
            {
                //Skip if status is null
                if (!booking.Status.HasValue)
                    continue;
                int priority;
                string notifMessage = GetPriority(booking, out priority);
                if (!string.IsNullOrEmpty(notifMessage))
                {
                    todaysNotifications.Add(new Notification()
                    {
                        BookingId = booking.Id,
                        NotificationText = notifMessage,
                        NotificationType = "BookingProgressAlert",
                        Priority = priority,
                        CreatedOn = DateTime.Now,
                        IsRead = false

                    });
                }

            }
            _context.Notifications.RemoveRange(_context.Notifications.Where(n => n.CreatedOn.Date == today)); // Clear today's notifications



            // Payment verification Alerts
            var verifRequests = _context.ReceiptVerificationRequests.Where(p => p.Status == 1).ToList(); //1: verification pending
                if (verifRequests.Count > 0)
            {
                foreach (var request in verifRequests)
                {
                    todaysNotifications.Add(new Notification()
                    {
                        BookingId = request.Id,
                        NotificationText = "Payment verification request is pending. Please verify.",
                        NotificationType = "VerifRequestAlert",
                        Priority = 1,
                        CreatedOn = DateTime.Now,
                        IsRead = false

                    });
                }
            }
            // allotment Letter   Alerts
            var alotmentLetterRequests = _context.AllotmentLetterRequests.Where(p => p.Status == 1).ToList(); //1: verification pending
            if (verifRequests.Count > 0)
            {
                foreach (var request in verifRequests)
                {
                    todaysNotifications.Add(new Notification()
                    {
                        BookingId = request.Id,
                        NotificationText = "Allotment Letter is  pending. Please get that ready and upload.",
                        NotificationType = "AllotmentLetterRequestAlert",
                        Priority = 1,
                        CreatedOn = DateTime.Now,
                        IsRead = false

                    });
                }
            }
            _context.Notifications.AddRange(todaysNotifications); // Add new notifications for today 
            _context.SaveChanges();
        }
        private string GetPriority(Booking  booking, out int priority)
        {
            string notificationMessage = "";
            priority = 1; // normal, 2: high, 3: critical
           
            if (booking.Status == 99)
            {
                priority = 1; // Cancelled
            }
            else if (booking.Status == 50) //Closed
            {
                priority = 1;
            }
            else
            {
                if (booking.Status == 18) //JDA Patta Applied
                {
                    DateTime jdaPattaAppliedOn = booking.JDAPattaAppliedOn.GetValueOrDefault();
                    int daysElapsed_jdaPattaAppliedOn = DateTime.Now.Subtract(jdaPattaAppliedOn).Days;
                    if (daysElapsed_jdaPattaAppliedOn > 7) //means 7 days elapsed after JDA patta applied
                    {
                        priority = 3;
                        notificationMessage = "JDA Patta applied more than 7 days ago. Please take necessary action.";
                    }
                    else if (daysElapsed_jdaPattaAppliedOn > 5)  //2 
                    {
                        priority = 2;
                        notificationMessage = "JDA Patta applied more than 5 days ago. Please follow up.";
                    }
                    else
                    {
                        priority = 1; //1 means in time
                        notificationMessage = "JDA Patta applied recently. No immediate action needed.";
                    }
                }
                else if (booking.Status == 16) //means Dokit Signed
                {
                    DateTime dokitSignedOn = booking.DokitSignDate.GetValueOrDefault();
                    int daysElapsed_dokitSignedOn = DateTime.Now.Subtract(dokitSignedOn).Days;
                    if (daysElapsed_dokitSignedOn > 1) //means 1 day  elapsed afterDokit signed applied
                    {
                        priority = 3;
                        notificationMessage = "Dokit signed more than 1 day ago. Please take necessary action.";
                    }
                    else
                    {
                        priority = 1; //Green means in time
                        notificationMessage = "Dokit signed recently. No immediate action needed.";
                    }
                }
                else if (booking.Status == 14) //Loan Sanctioned
                {
                    DateTime loanSanctionedOn = booking.LoanSanctionDate.GetValueOrDefault();
                    int daysElapsed_loanSanctionedOn = DateTime.Now.Subtract(loanSanctionedOn).Days;
                    if (daysElapsed_loanSanctionedOn > 7) //means 7 days elapsed after JDA patta applied
                    {
                        priority = 3;
                        notificationMessage = "Loan sanctioned more than 7 days ago. Please take necessary action.";
                    }
                    else if (daysElapsed_loanSanctionedOn > 5)  //Orange 
                    {
                        priority = 2;
                        notificationMessage = "Loan sanctioned more than 5 days ago. Please follow up.";
                    }
                    else
                    {
                        priority = 1;   //Green means in time
                        notificationMessage = "Loan sanctioned recently. No immediate action needed.";
                    }
                }
                else if (booking.Status == 1) //Booked
                {
                    DateTime bookedOn = booking.BookingDate;
                    int daysElapsed_bookedOn = DateTime.Now.Subtract(bookedOn).Days;
                    if (daysElapsed_bookedOn > 7) //means 7 days elapsed after booking
                    {
                        priority = 3;
                        notificationMessage = "Booking made more than 7 days ago. Please take necessary action.";
                    }
                    else if (daysElapsed_bookedOn > 5)  //Orange 
                    {
                        notificationMessage = "Booking made more than 5 days ago. Please follow up.";
                        priority = 2;
                    }
                    else
                    {
                        priority = 1; //Green means in time
                        notificationMessage = "Booking made recently. No immediate action needed.";
                    }
                }
            }

            return notificationMessage;
        }
    }
}