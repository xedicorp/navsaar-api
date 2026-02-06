

using navsaar.api.Infrastructure;
using navsaar.api.Models;
using navsaar.api.ViewModels;

namespace navsaar.api.Repositories
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly AppDbContext _context;
        public DocumentRepository(AppDbContext context)
        {
            _context = context;
        }



        public async Task<bool> Upload(UploadDocumentRequest request)
        {
            if (request.File == null || request.File.Length == 0)
            {
                return false;
            }

            // Define the path where the file will be saved
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Generate a unique file name to avoid conflicts
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + request.File.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await request.File.CopyToAsync(stream);
            }


            Document document = new Document();
            document.BookingId = request.BookingId;
            document.DocumentTypeId = request.DocumentTypeId;
            document.Notes = request.Notes;
            document.UploadedOn = DateTime.Now;
            document.UploadedBy = 1; //TODO: Change to logged in user
            document.FilePath = uniqueFileName;
            document.FileName = request.File.FileName;
            _context.Documents.Add(document);
            _context.SaveChanges();
            return true;
        }

        public List<DocumentModel> GetAllByBookingId(int bookingId)
        {
            var q = from p in _context.Documents
                    join d in _context.DocumentTypes on p.DocumentTypeId equals d.Id
                    join u in _context.Users on p.UploadedBy equals u.Id
                    where p.BookingId == bookingId
                    select new DocumentModel
                    {
                        DocumentId = p.Id,
                        BookingId = p.BookingId,
                        DocumentTypeName = d.Name,
                        Notes = p.Notes,
                        UploadedOn = p.UploadedOn,
                        Url = p.FilePath,
                        UploadedBy = u.UserName,
                        DocumentTypeId = d.Id,
                        FileName = p.FileName
                    };
            return q.ToList();
        }

        public Document GetById(int id)
        {
            return _context.Documents.Find(id);
        }

        public List<WorkflowDocType> GetDocTypesByWorkflow(int workflowTypeId)
        {
         return( from p in _context.WorkflowDocTypes                    
                     where p.WorkflowTypeId == workflowTypeId
                     select new WorkflowDocType
                     {
                         Id = p.Id,
                         WorkflowTypeId = p.WorkflowTypeId,
                         DocumentTypeId = p.DocumentTypeId  
                     }).ToList();
        }
    }
}
