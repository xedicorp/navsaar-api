using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace navsaar.api.Models
{
    [Table("tblAssociates")]
    public class AssociateInfo
    {
        [Key]
        public long ID { get; set; }

        [StringLength(50)]
        public string? UserName { get; set; }

        [StringLength(100)]
        public string? FirstName { get; set; }

        [StringLength(100)]
        public string? LastName { get; set; }

        [StringLength(255)]
        public string? Email { get; set; }

        [StringLength(255)]
        public string? Password { get; set; }

        [StringLength(20)]
        public string? PANCardNo { get; set; }

        [StringLength(20)]
        public string? AadhaarNo { get; set; }

        public string? BankPassbookImage { get; set; }

        public string? SponsorID { get; set; }
        public string? RERA { get; set; }

        public bool? IsActive { get; set; } = false;

        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; } = DateTime.Now;

        [StringLength(50)]
        public string? AccountNumber { get; set; }

        [StringLength(100)]
        public string? AccountName { get; set; }

        [StringLength(100)]
        public string? BankName { get; set; }

        [StringLength(20)]
        public string? BankIFSC { get; set; }

        [StringLength(100)]
        public string? BankBranch { get; set; }
        public bool? IsApproved { get; set; }
        [StringLength(500)]
        public string? RejectReason { get; set; }
        [StringLength(100)]
        public string? NomineeName { get; set; }
        [StringLength(20)]
        public string? NomineeContactNo { get; set; }
        [StringLength(100)]
        public string? NomineeRelation { get; set; }

        [StringLength(20)]
        public string? ContactNo { get; set; }

        [StringLength(100)]
        public string? LeaderName { get; set; }
        public DateTime? DOB { get; set; }
        public DateTime? AnniversaryDate { get; set; }

        [StringLength(255)]
        public string? RERACertificateFile { get; set; }

        [StringLength(255)]
        public string? PhotoFile { get; set; }

        [StringLength(50)]
        public string? PassportNo { get; set; }

        [StringLength(255)]
        public string? PassportFile { get; set; }

        [StringLength(255)]
        public string? BankDocumentFile { get; set; }

        [StringLength(20)]
        public string? LeaderContactNo { get; set; }


    }
}
