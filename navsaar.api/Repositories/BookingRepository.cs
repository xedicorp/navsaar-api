

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
        public async Task Save(CreateUpdateBookingModel booking)
        {
            var entity = new Models.Booking();

            if (booking.File != null)
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
                      booking.File.CopyTo(stream);
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

            if (booking.Id == 0)
            {
                _context.Bookings.Add(entity);
            }
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
        public bool UpdateLoanSanctionStatus(UpdateLoanSanctionStatusRequest request)
        {
            //Stage5
            var entity = _context.Bookings.Find(request.BookingId);
            if (entity == null)
            {
                return false;
            }
            entity.IsLoanSanctioned = request.IsLoanSanctioned;
            entity.LoanSanctionDate = request.LoanSanctionDate;  
            entity.LoanSanctionNotes = request.Notes;
            entity.CurrentStage = 5;
            _context.SaveChanges();
            return true;
        }
        public bool UpdateMarkFileCheckStatus(UpdateMarkFileCheckStatusRequest request)
        {
            //Stage5
            var entity = _context.Bookings.Find(request.BookingId);
            if (entity == null)
            {
                return false;
            }
            entity.IsCompletedOnAllSides = request.IsCompletedOnAllSides;
            entity.CompletionDate = request.CompletionDate;
            entity.MarkFileCheckNotes = request.Notes;
            entity.CurrentStage = 6;
            _context.SaveChanges();
            return true;
        }
        public async Task UploadOriginalATT(UploadOriginalATTRequest request)
        {
            var entity = new Models.Booking();

            if (request.File != null)
            {
                // Define the path where the file will be saved
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                string fileName = Guid.NewGuid().ToString() + "_" + request.File.FileName;
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    request.File.CopyTo(stream);
                }
            }

            if (request.BookingId != 0)
            {
                entity = _context.Bookings.Find(request.BookingId);
                entity.CurrentStage = 7;
            }

            entity.OriginalATTPath = request.File != null ? entity.ChequeFilePath : entity.ChequeFilePath;
            entity.OriginalATTNotes=request.Notes;
       
            _context.SaveChanges();
        }

        public bool UpdateDokitSigningStatus(UpdateDokitSigningStatusRequest request)
        {
            var entity = _context.Bookings.Find(request.BookingId);
            if (entity == null)
            {
                return false;
            }
            entity.IsDokitSigned = request.IsDokitSigned;
            entity.DokitSignDate = request.DokitSignDate;
            entity.IsJDAFileSigned = request.IsJDAFileSigned;
            entity.JDAFileSignDate = request.JDAFileSignDate;
            entity.DokitSigingNotes = request.Notes;
            entity.CurrentStage = 8;
            _context.SaveChanges();
            return true;
        }
        public async Task UploadBankDD(UploadBankDDRequest request)
        {
            var entity = new Models.Booking();

            if (request.File != null)
            {
                // Define the path where the file will be saved
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                string fileName = Guid.NewGuid().ToString() + "_" + request.File.FileName;
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    request.File.CopyTo(stream);
                }
            }

            if (request.BookingId != 0)
            {
                entity = _context.Bookings.Find(request.BookingId);
                entity.CurrentStage = 9;
            }

            entity.BankDDPath = request.File != null ? entity.BankDDPath : entity.BankDDPath;
            entity.DDAmount = request.DDAmount;
            entity.DDNo = request.DDNo;
            entity.DDNotes = request.DDNotes;
            _context.SaveChanges();
        }
        public bool UpdateJDAPattaStatus(UpdateJDAPattaStatusRequest request)
        {
            var entity = _context.Bookings.Find(request.BookingId);
            if (entity == null)
            {
                return false;
            }
            entity.IsJDAPattaApplied = request.IsJDAPattaApplied;
            entity.JDAPattaAppliedOn = request.JDAPattaAppliedOn;
            entity.IsJDAPattaRegistered = request.IsJDAPattaRegistered;
            entity.JDAPattaRegisteredOn = request.JDAPattaRegisteredOn;
            entity.IsJDAPattaGivenToBank = request.IsJDAPattaGivenToBank;
            entity.JDAPattaGivenToBankOn = request.JDAPattaGivenToBankOn;
            entity.IsDDReceivedFromBank = request.IsDDReceivedFromBank;
            entity.DDReceivedFromBankOn = request.DDReceivedFromBankOn;
            entity.JDAPattaNotes = request.Notes;
            entity.CurrentStage = 8;
            _context.SaveChanges();
            return true;
        }
        public bool UpdateBankDDStatus(UpdateBankDDStatusRequest request)
        {
            var entity = _context.Bookings.Find(request.BookingId);
            if (entity == null)
            {
                return false;
            }
            entity.IsDDSubmittedToBank = request.IsDDSubmittedToBank;
            entity.DDClearedOn = request.DDClearedOn;            
            entity.DDUpdateNotes = request.Notes;
            entity.CurrentStage = 8;
            _context.SaveChanges();
            return true;
        }
    }
}
