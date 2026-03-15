using System.ComponentModel.DataAnnotations.Schema;

namespace navsaar.api.Models
{
    [Table("tblFcmTokens")]
    public class FcmToken
    {
        public int Id { get; set; }
        public string MobileNo { get; set; }
        public string Token { get; set; }
      

    }
}
