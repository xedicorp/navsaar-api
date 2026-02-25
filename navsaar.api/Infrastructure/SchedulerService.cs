using Microsoft.EntityFrameworkCore;
using Hangfire;
using navsaar.api.Models;

namespace navsaar.api.Infrastructure
{
    public class SchedulerService : ISchedulerService
    {
        private readonly AppDbContext _context;

        //STATUS → NOTIFICATION MESSAGE
        private static readonly Dictionary<int, string> StatusNotificationMap =
            new()
            {
                { 7,  "On Hold for Initial Payment Confirmation" },
                { 8,  "On Hold for Draft Preparation" },
                { 10, "On Hold for Allotment Letter Preparation" },
                { 9,  "Draft Prepared" },
                { 11, "Allotment Letter Prepared" },
                { 14, "Loan Sanction Done" },
                { 30, "Bank DD Received" },
                { 32, "DD Submitted to Bank" },
                { 34, "Bank DD Cleared" }
            };

        //STATUS → PRIORITY
        // 1 = Critical | 2 = Important | 3 = Info
        private static readonly Dictionary<int, int> StatusPriorityMap =
            new()
            {
                { 7,  2 }, // On Hold – Important
                { 8,  2 }, // On Hold – Important
                { 10, 2 }, // On Hold – Important
                { 9,  3 }, // Draft Prepared – Info
                { 11, 3 }, // Allotment Prepared – Info
                { 14, 1 }, // Loan Sanction – CRITICAL
                { 30, 2 }, // Bank DD Received – Important
                { 32, 2 }, // DD Submitted – Important
                { 34, 3 }  // DD Cleared – Info
            };

        //STATUSES THAT SHOULD NEVER GENERATE NOTIFICATIONS
        private static readonly int[] IgnoredStatuses =
        {
            50, // Closed
            99, // Cancelled
            15  // File Marked Completed
        };

        public SchedulerService(AppDbContext context)
        {
            _context = context;
        }

        // Prevent multiple parallel executions by Hangfire
        [DisableConcurrentExecution(300)]
        public void GenerateNofications()
        {
            var today = DateTime.Today;

            foreach (var booking in _context.Bookings.ToList())
            {
                //Skip if status is null
                if (!booking.Status.HasValue)
                    continue;

                int status = booking.Status.Value;

                //Skip ignored statuses
                if (IgnoredStatuses.Contains(status))
                    continue;

                //Send notification only for mapped statuses
                if (StatusNotificationMap.ContainsKey(status))
                {
                    string message = StatusNotificationMap[status];
                    int priority = StatusPriorityMap[status];

                    CreateNotificationOnce(
                        booking.Id,
                        message,
                        "STATUS_UPDATE",
                        priority,
                        today
                    );
                }
            }

            _context.SaveChanges();
        }

        private void CreateNotificationOnce(
            int bookingId,
            string text,
            string type,
            int priority,
            DateTime today)
        {
            //Correct duplicate prevention (SQL-safe)
            bool exists = _context.Notifications.Any(n =>
                n.BookingId == bookingId &&
                n.NotificationType == type &&
                n.CreatedOn >= today &&
                n.CreatedOn < today.AddDays(1)
            );

            if (exists)
                return;

            _context.Notifications.Add(new Notification
            {
                BookingId = bookingId,
                NotificationText = text,
                NotificationType = type,
                Priority = priority,
                CreatedOn = DateTime.Now,
                IsRead = false
            });
        }
    }
}