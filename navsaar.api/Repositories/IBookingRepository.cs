

using navsaar.api.ViewModels;

namespace navsaar.api.Repositories
{
    public interface IBookingRepository
    {
        List<BookingInfo> List();
        void Save(CreateUpdateBookingModel booking);
        bool UpdateInitialPayment(UpdateInitialPaymentRequest request); //Stage2
        bool UploadLoanDocument(UploadLoanDocumentRequest request); //Stage 3

        List<DocumentModel> GetAllUploadedLoanDocuments(int bookingId);

        bool UpdateLoginStatus(UpdateLoginStatusRequest request); //Stage 4
        bool UpdateDraftPerparationStatus(DraftPerparationStatusRequest request); //Stage 5
        
    }
}
