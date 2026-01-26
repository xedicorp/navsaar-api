

using navsaar.api.ViewModels;
using navsaar.api.ViewModels.Receipt;

namespace navsaar.api.Repositories
{
    public interface IReceiptRepository
    {
        List<ReceiptInfo> List();
        List<ReceiptInfo> ListByBookingId(int bookingId);
        bool Save(CreateUpdateReceiptModel model);
        List<VerificationRequestInfo> VerificationRequests();
        bool SendVerificationRequest(VerifRequest model);
        bool Verify(VerifReceiptRequest model);
    }
}
