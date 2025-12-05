

using navsaar.api.Infrastructure;
using navsaar.api.Models;
using navsaar.api.ViewModels;

namespace navsaar.api.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly AppDbContext _context;
        public BookingRepository(AppDbContext context)
        {
            _context = context;
        }
        public async void Save(CreateUpdateBookingModel booking)
        {
            var entity = new Models.Booking();

            if(booking.File!=null)
            {
                // Define the path where the file will be saved
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                string fileName = Guid.NewGuid().ToString() + "_" + booking.File.FileName;  
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await booking.File.CopyToAsync(stream);
                }
            }

            if (booking.Id != 0)
            {
                entity = _context.Bookings.Find(booking.Id);
                entity.CurrentStage = 1;
            }

            entity.ChequeFilePath = booking.File != null ? entity.ChequeFilePath : entity.ChequeFilePath;
            entity.TownshipId = booking.TownshipId;
            entity.PlotNo = booking.PlotNo;
            entity.PlotSize = booking.PlotSize;
            entity.BookingDate = booking.BookingDate;
            entity.ClientName = booking.ClientName;
            entity.ClientEmail = booking.ClientEmail;
            entity.ClientContactNo = booking.ClientContactNo;
            entity.AssociateName = booking.AssociateName;
            entity.AssociateReraNo = booking.AssociateReraNo;
            entity.AssociateContactNo = booking.AssociateContactNo;
            entity.LeaderName = booking.LeaderName;
            entity.WorkflowTypeId = 1; // Booking Workflow

            _context.Bookings.Add(entity);
            _context.SaveChanges();
        }
        public List<BookingInfo> List()
        {
            return (from p in _context.Bookings
                    join t in _context.Townships on p.TownshipId equals t.Id
                    select new BookingInfo
                    {
                        Id = p.Id,
                        TownshipName = t.Name,
                        PlotNo = p.PlotNo,
                        PlotSize = p.PlotSize, 
                        BookingDate = p.BookingDate,
                        ClientName = p.ClientName,
                        ClientEmail = p.ClientEmail,
                        ContactNo = p.ClientContactNo,
                        AssociateName = p.AssociateName,
                        AssociateReraNo = p.AssociateReraNo,
                        AssociateContactNo = p.AssociateContactNo,
                        LeaderName = p.LeaderName
                    }).ToList();

        }

        public bool UpdateInitialPayment(UpdateInitialPaymentRequest request)
        {
            var entity = _context.Bookings.Find(request.BookingId);
            if (entity == null)
            {
                return false;
            }
            entity.PaymentMode = request.PaymentMode;
            entity.Amount_2 = request.Amount;
            entity.TransNo = request.TransNo;
            entity.DateOfTransfer = request.DateOfTransfer;
            entity.IsPaymentVerified = request.IsPaymentVerified;
            entity.Notes_2 = request.Notes;
            entity.CurrentStage = 2;
            _context.SaveChanges();
            return true;
        }

        public bool UpdateLoginStatus(UpdateLoginStatusRequest request)
        {
            //Stage3
            var entity = _context.Bookings.Find(request.BookingId);
            if (entity == null)
            {
                return false;
            }
            entity.DateOfLogin = request.DateOfLogin;
            entity.BankName = request.BankName;
            entity.BranchName = request.BranchName;
            entity.LoginRefNo = request.LoginRefNo;           
            entity.Notes_3 = request.Notes;
            entity.CurrentStage = 3;
            _context.SaveChanges();
            return true;
        }

        public bool UpdateDraftPerparationStatus(DraftPerparationStatusRequest request)
        {
            //Stage4
            var entity = _context.Bookings.Find(request.BookingId);
            if (entity == null)
            {
                return false;
            }
            entity.IsDraftPrepared = request.IsDraftPrepared;
            entity.DraftPreparedOn = request.DraftPreparedOn;
            entity.IsDraftGivenToBank = request.IsDraftGivenToBank;
            entity.DraftGivenToBankOn = request.DraftGivenToBankOn;
            entity.Notes_4 = request.Notes;
            entity.CurrentStage = 4;
            _context.SaveChanges();
            return true;
        }

        public bool UploadLoanDocument(UploadLoanDocumentRequest request)
        {
            Document document = new Document();
            document.BookingId = request.BookingId;
            document.DocumentTypeId = request.DocumentTypeId;
            document.Notes = request.Notes;
            document.UploadedOn = DateTime.Now;
            document.UploadedBy = 1; //TODO: Change to logged in user
            document.FilePath = "/Uploads/" + request.File.FileName;
            _context.Documents.Add(document);
            _context.SaveChanges();
            return true;
        }

        public List<DocumentModel> GetAllUploadedLoanDocuments(int bookingId)
        {
            throw new NotImplementedException();
        }
    }
}
