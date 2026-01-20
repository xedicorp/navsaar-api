

using navsaar.api.ViewModels;
using navsaar.api.ViewModels.Booking;

namespace navsaar.api.Repositories
{
    public interface IBookingRepository
    {
        List<BookingInfo> List();
         BookingInfo  GetById(int bookingId);
        List<BookingProgressModel> GetBookingProgress(int bookingId);
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
        bool ChangePlot(ChangePlotRequest request);
        bool Cancel(BookingCancelRequest request);

        List<BookingInfo> Search(int? townshipId, int? bookingType, int? associateId, DateTime? fromDate);
    }
}
