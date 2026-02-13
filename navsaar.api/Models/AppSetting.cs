using System.ComponentModel.DataAnnotations.Schema;
namespace navsaar.api.Models
{
    [Table("tblAppSettings")]

    public class AppSetting
    {
        public int Id { get; set; }
        public string? LogoUrl { get; set; }
        public string? ApiUrl { get; set; }
        public string? ColorTheme { get; set; }
        public string? CompanyName { get; set; }

    }
}