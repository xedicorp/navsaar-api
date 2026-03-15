using System.ComponentModel.DataAnnotations.Schema;

namespace navsaar.api.Models
{
    [Table("tblMobileOtps")]
    public class MobileOtp
    {
        public int Id { get; set; }
        public string MobileNo { get; set; }
        public string Otp { get; set; }
        public DateTime ExpiresAt { get; set; }

    }
}
