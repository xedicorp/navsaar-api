using navsaar.api.ViewModels.Refund;

namespace navsaar.api.Repositories.Refunds
{
    public interface IRefundRepository
    {
        public List<RefundInfo> List();
        bool SaveStatus(SaveRefundStatusModel model);
        List<RefundStatusInfo> StatusLogs(int refundRequestId); 
    }
}
