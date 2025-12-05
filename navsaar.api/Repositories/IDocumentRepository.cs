

using navsaar.api.ViewModels;

namespace navsaar.api.Repositories
{
    public interface IDocumentRepository
    {
        Task<bool> Upload(UploadDocumentRequest request); //Stage 3 
        List<DocumentModel> GetAllByBookingId(int bookingId); 
    }
}
