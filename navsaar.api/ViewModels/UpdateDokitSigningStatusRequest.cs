namespace navsaar.api.ViewModels
{
    public class UpdateDokitSigningStatusRequest
    {
        public int BookingId { get; set; }
        public bool? IsDokitSigned { get; set; }
        public DateTime? DokitSignDate { get; set; }
        public bool? IsJDAFileSigned { get; set; }
        public DateTime? JDAFileSignDate { get; set; }
        public string? Notes { get; set; }
    }
}
