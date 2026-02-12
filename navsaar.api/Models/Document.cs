using System.ComponentModel.DataAnnotations.Schema;

namespace navsaar.api.Models
{
    [Table("tblDocuments")]
    public class Document
    {
        public int Id { get; set; }
        public int? BookingId { get; set; }
        public int? DocumentTypeId { get; set; }
        public string? FilePath { get; set; }
        public string? FileName { get; set; }
        public DateTime UploadedOn { get; set; }
        public int? UploadedBy { get; set; }
        public string? Notes { get; set; }
        public bool IsDraft { get; set; }
        public bool IsATT { get; set; }
        public bool IsAllotment { get; set; }
    }
}
