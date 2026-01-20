using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace navsaar.api.Models
{
    [Table("tblAssociates")]
    public class Associate
    {
        [Key]
        public long ID { get; set; }

        [Required, StringLength(50)]
        public string UserName { get; set; }

        [Required, StringLength(100)]
        public string FirstName { get; set; }

        [Required, StringLength(100)]
        public string LastName { get; set; }

        [Required, StringLength(255)]
        public string Email { get; set; }

        [Required, StringLength(255)]
        public string Password { get; set; }

        [Required, StringLength(20)]
        public string PANCardNo { get; set; }

        [Required, StringLength(20)]
        public string AadhaarNo { get; set; }

        public string BankPassbookImage { get; set; }

        public string SponsorID { get; set; }
        public string RERA { get; set; }

        public bool IsActive { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [StringLength(50)]
        public string AccountNumber { get; set; }

        [StringLength(100)]
        public string AccountName { get; set; }

        [StringLength(100)]
        public string BankName { get; set; }

        [StringLength(20)]
        public string BankIFSC { get; set; }

        [StringLength(100)]
        public string BankBranch { get; set; }
        public bool? IsApproved { get; set; }
        [StringLength(500)]
        public string RejectReason { get; set; }
        [StringLength(100)]
        public string NomineeName { get; set; }
        [StringLength(20)]
        public string NomineeContactNo { get; set; }
        [StringLength(100)]
        public string NomineeRelation { get; set; }
    }
}
