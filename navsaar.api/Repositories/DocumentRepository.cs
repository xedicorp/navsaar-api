

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
                  request.File.CopyTo(stream);
            }


            Document document = new Document();
            document.BookingId = request.BookingId;
            document.DocumentTypeId = request.DocumentTypeId;
            document.Notes = request.Notes;
            document.UploadedOn = DateTime.UtcNow;
            document.UploadedBy = 1; //TODO: Change to logged in user
            document.FilePath = uniqueFileName;
            document.FileName = request.File.FileName;
            document.IsDraft = request.IsDraft;
            document.IsATT = request.IsATT;
            document.IsAllotment = request.IsAllotment;
            _context.Documents.Add(document);
            _context.SaveChanges();


            //Get pending docs for Booking
            var booking = _context.Bookings.Find(request.BookingId);
            var requiredDocs = GetDocTypesByWorkflow(booking.WorkflowTypeId.GetValueOrDefault());

            var uploadedDocs = _context.Documents.Where(p => p.BookingId == request.BookingId).ToList();
            //Check if all required docs are uploaded
            bool isAllUploaded = true;
            foreach (var doc in requiredDocs)
            {

                if (!uploadedDocs.Any(p => p.DocumentTypeId == doc.DocumentTypeId))
                {
                    isAllUploaded = false; // Not all required docs are uploaded, but upload is successful
                }
            }

            if (isAllUploaded)
            {
                booking.IsReqDocsUploaded = true;
                booking.ReqDocsUploadedOn = DateTime.UtcNow;
                booking.Status = 4;
                _context.Bookings.Update(booking);
                _context.SaveChanges();
            }
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
                        FileName = p.FileName,
                        IsDraft = p.IsDraft,
                        IsATT = p.IsATT,
                        IsAllotment = p.IsAllotment,
                        ClientPhotoUrl = p.DocumentTypeId == 4 ? p.FilePath: null
                    };
            var data = q.ToList();

            var istZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

            foreach (var item in data)
            {
                if (item.UploadedOn.HasValue)
                {
                    item.UploadedOn = TimeZoneInfo.ConvertTimeFromUtc(item.UploadedOn.Value, istZone);
                }
            }

            return data;
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
        public bool Delete(int id)
        {
            var document = _context.Documents.FirstOrDefault(x => x.Id == id);

            if (document == null)
                return false;

            // Delete file from Uploads folder
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            var fullFilePath = Path.Combine(uploadsFolder, document.FilePath);

            if (System.IO.File.Exists(fullFilePath))
            {
                System.IO.File.Delete(fullFilePath);
            }

            // Remove from database
            _context.Documents.Remove(document);
            _context.SaveChanges();

            return true;
        }
    }
}
