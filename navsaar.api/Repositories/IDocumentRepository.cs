

using navsaar.api.Models;
using navsaar.api.ViewModels;

namespace navsaar.api.Repositories
{
    public interface IDocumentRepository
    {
        Task<bool> Upload(UploadDocumentRequest request); //Stage 3 
        List<DocumentModel> GetAllByBookingId(int bookingId);
        List<WorkflowDocType> GetDocTypesByWorkflow(int workflowTypeId);
        Document GetById(int id);
    }
}
