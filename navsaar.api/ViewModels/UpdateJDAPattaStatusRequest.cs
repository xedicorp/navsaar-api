namespace navsaar.api.ViewModels
{
    public class UpdateJDAPattaStatusRequest
    {
        public int BookingId { get; set; }
        public bool? IsJDAPattaApplied { get; set; }
        public DateTime? JDAPattaAppliedOn { get; set; }
        public bool? IsJDAPattaRegistered { get; set; }
        public DateTime? JDAPattaRegisteredOn { get; set; }
        public bool? IsJDAPattaGivenToBank { get; set; }
        public DateTime? JDAPattaGivenToBankOn { get; set; }
        public bool? IsDDReceivedFromBank { get; set; }
        public DateTime? DDReceivedFromBankOn { get; set; }
        public string? Notes { get; set; }
    }
}
