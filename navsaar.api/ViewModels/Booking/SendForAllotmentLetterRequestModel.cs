namespace navsaar.api.ViewModels 
{
    public class SendForAllotmentLetterRequestModel
    {
        public int UserId { get; set; }
        public int BookingId { get; set; }
        public string? Notes { get; set; }
        public bool IsOriginalAgreement { get; set; }
        public string ApplicantName { get; set; } = string.Empty;
        public string RelativeName { get; set; } = string.Empty;
        public string? RelationType { get; set; }

        public string Address { get; set; } = string.Empty;
        public string ContactNo { get; set; } = string.Empty;
    }
}
