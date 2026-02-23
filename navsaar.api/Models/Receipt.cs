using System.ComponentModel.DataAnnotations.Schema;

namespace navsaar.api.Models
{
  [Table("tblReceipts")]
    public class Receipt
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public decimal Amount { get; set; }
        public DateTime ReceiptDate { get; set; }
        public int ReceiptMethod { get; set; }
        public string TransactionId { get; set; }
        public string? BankName { get; set; }
        public string? ChequeNo { get; set; }
        public int? Status { get; set; }
        public string? Notes { get; set; }
        public string? receiptImage { get; set; }
        public int? CreatedBy { get; set; } 

    }
}
