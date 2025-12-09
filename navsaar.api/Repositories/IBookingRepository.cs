

using navsaar.api.ViewModels;

namespace navsaar.api.Repositories
{
    public interface IBookingRepository
    {
        List<BookingInfo> List();
        Task Save(CreateUpdateBookingModel booking);
        bool UpdateInitialPayment(UpdateInitialPaymentRequest request); //Stage2
       

        bool UpdateLoginStatus(UpdateLoginStatusRequest request); //Stage 4
        bool UpdateDraftPerparationStatus(DraftPerparationStatusRequest request); //Stage 5

        bool UpdateLoanSanctionStatus(UpdateLoanSanctionStatusRequest request); //Stage 5

        bool UpdateMarkFileCheckStatus(UpdateMarkFileCheckStatusRequest request); //Stage 6

        Task UploadOriginalATT(UploadOriginalATTRequest request);

        bool UpdateDokitSigningStatus(UpdateDokitSigningStatusRequest request); //Stage 6
        Task UploadBankDD(UploadBankDDRequest request);

        bool UpdateJDAPattaStatus(UpdateJDAPattaStatusRequest request);

        bool UpdateBankDDStatus(UpdateBankDDStatusRequest request);

    }
}
