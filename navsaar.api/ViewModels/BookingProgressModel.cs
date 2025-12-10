namespace navsaar.api.ViewModels
{
    public class BookingProgressModel
    {
        public DateTime ProgressDate { get; set; }
        public string ProgressDetails { get; set; }
        public int DaysFromBooking { get; set; } = 0;
    }
}
