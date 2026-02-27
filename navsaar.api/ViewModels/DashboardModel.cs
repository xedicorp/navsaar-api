namespace navsaar.api.ViewModels
{
    public class DashboardModel
    {
        public NotificationCount NotificationCount { get; set; }
        public int PendingDraftRequestCount { get; set; }
        public int PendingReceiptVerificationCount { get; set; }


    }

    public class NotificationCount
    {
        public int HighPriority { get; set; }
        public int MediumPriority { get; set; }
        public int LowPriority { get; set; }
    }
    }
