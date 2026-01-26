namespace navsaar.api.ViewModels.Receipt
{
    public class VerifReceiptRequest
    {
        public int ReceiptId { get; set; }
        public int Status { get; set; }  //1: Approved 2: Rejected
        public string? RejectReason { get; set; }
    }
}
