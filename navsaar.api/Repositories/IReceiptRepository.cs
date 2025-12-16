

using navsaar.api.ViewModels;

namespace navsaar.api.Repositories
{
    public interface IReceiptRepository
    {
        List<ReceiptInfo> List();
        List<ReceiptInfo> ListByBookingId(int bookingId);
        bool Save(CreateUpdateReceiptModel model);
    }
}
