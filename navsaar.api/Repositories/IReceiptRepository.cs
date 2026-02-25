

using navsaar.api.ViewModels;
using navsaar.api.ViewModels.Receipt;

namespace navsaar.api.Repositories
{
    public interface IReceiptRepository
    {
        List<ReceiptInfo> List();
        List<ReceiptInfo> ListByBookingId(int bookingId);
        Task<bool> Save(CreateUpdateReceiptModel model);
        VerificationRequestApiResponse VerificationRequests();
        bool SendVerificationRequest(VerifRequest model);
        bool Verify(VerifReceiptRequest model);
    }
}
