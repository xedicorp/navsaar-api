

using navsaar.api.ViewModels;

namespace navsaar.api.Repositories
{
    public interface IBookingRepository
    {
        List<BookingInfo> List();
        void Save(CreateUpdateBookingModel booking);
        bool UpdateInitialPayment(UpdateInitialPaymentRequest request); //Stage2
       

        bool UpdateLoginStatus(UpdateLoginStatusRequest request); //Stage 4
        bool UpdateDraftPerparationStatus(DraftPerparationStatusRequest request); //Stage 5

        bool UpdateLoanSanctionStatus(UpdateLoanSanctionStatusRequest request); //Stage 5

        

    }
}
