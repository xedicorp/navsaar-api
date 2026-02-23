using System.ComponentModel.DataAnnotations.Schema;

namespace navsaar.api.Models
{
    [Table("tblBanks")]
    public class Bank
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}