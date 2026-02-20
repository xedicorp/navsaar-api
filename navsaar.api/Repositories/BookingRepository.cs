

using Azure.Core;
using Microsoft.EntityFrameworkCore;
using navsaar.api.Infrastructure;
using navsaar.api.Models;
using navsaar.api.Services;
using navsaar.api.ViewModels;
using navsaar.api.ViewModels.Booking;
using Twilio.Rest.Taskrouter.V1.Workspace.TaskQueue;

namespace navsaar.api.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly AppDbContext _context;
        private readonly IReceiptRepository _receiptRepository;
        private readonly IDocumentRepository _documentRepository;
        IWhatsAppService _whatsAppService;
        public BookingRepository(AppDbContext context, IReceiptRepository receiptRepository,
                IDocumentRepository documentRepository, IWhatsAppService whatsAppService)
        {
            _context = context;
            _receiptRepository = receiptRepository;
            _documentRepository = documentRepository;
            _whatsAppService = whatsAppService;
        }
        public async Task<int> Save(CreateUpdateBookingModel booking)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                bool isNew = false;
                Booking entity;

                if (booking.Id > 0)
                {
                    entity = await _context.Bookings.FindAsync(booking.Id);
                    if (entity == null)
                        return 0;

                    entity.CurrentStage = 1;
                }
                else
                {
                    entity = new Booking();
                    isNew = true;
                    await _context.Bookings.AddAsync(entity);
                }

                // ========================
                // BOOKING DATA
                // ========================

                entity.TownshipId = booking.TownshipId;
                entity.PlotNo = booking.PlotNo;
                entity.PlotSize = booking.PlotSize;
                entity.BookingDate = booking.BookingDate;
                entity.ClientName = booking.ClientName;
                entity.ClientEmail = booking.ClientEmail;
                entity.ClientContactNo = booking.ClientContactNo;
                entity.ClientAddress = booking.ClientAddress;
                entity.AssociateName = booking.AssociateName;
                entity.AssociateReraNo = booking.AssociateReraNo;
                entity.AssociateContactNo = booking.AssociateContactNo;
                entity.LeaderName = booking.LeaderName;
                entity.WorkflowTypeId = booking.WorkflowTypeId;
                entity.PlotId = booking.PlotId;
                entity.AgreementValue = booking.AgreementValue;
                entity.TotalAgreementValue = booking.TotalAgreementValue;
                entity.Status = 1;
                entity.LastStatusChangedBy = 1;
                entity.LastStatusChangedOn = DateTime.Now;
                entity.LeaderContactNo = booking.LeaderContactNo;   
                entity.RelationType = booking.RelationType;         
                entity.RelationName = booking.RelationName;         


                await _context.SaveChangesAsync(); // BookingId generated here

                // ========================
                // UPDATE PLOT STATUS
                // ========================

                if (isNew && booking.PlotId > 0)
                {
                    var plot = await _context.Plots.FindAsync(booking.PlotId);

                    if (plot != null)
                    {
                        plot.Status = 2; // Booked

                        var hold = await _context.PlotHoldRequests
                            .FirstOrDefaultAsync(x => x.PlotId == booking.PlotId && !x.IsDelete);

                        if (hold != null)
                            hold.IsDelete = true;

                        await _context.SaveChangesAsync();
                    }
                }

                // ========================
                // CREATE INITIAL RECEIPT
                // ========================

                if (isNew && booking.InitialAmount.HasValue && booking.InitialAmount > 0)
                {
                    var receipt = new Receipt
                    {
                        BookingId = entity.Id,
                        Amount = booking.InitialAmount.Value,
                        ReceiptDate = booking.InitialPaymentDate ?? DateTime.Now,
                        ReceiptMethod = booking.InitialReceiptMethod ?? 1,
                        TransactionId = booking.InitialTransactionId,
                        BankName = booking.InitialBankName,
                        ChequeNo = booking.InitialChequeNo,
                        Notes = booking.InitialNotes,
                        Status = booking.InitialReceiptStatus ?? 1
                    };

                    // ========================
                    // SAVE RECEIPT IMAGE
                    // ========================

                    if (booking.InitialReceiptImage != null &&
                        booking.InitialReceiptImage.Length > 0)
                    {
                        var uploadsFolder = Path.Combine(
                            Directory.GetCurrentDirectory(),
                            "Uploads",
                            "Receipts");

                        if (!Directory.Exists(uploadsFolder))
                            Directory.CreateDirectory(uploadsFolder);

                        var fileName = Guid.NewGuid() + "_" +
                                       booking.InitialReceiptImage.FileName;

                        var filePath = Path.Combine(uploadsFolder, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await booking.InitialReceiptImage.CopyToAsync(stream);
                        }

                        receipt.receiptImage = fileName;
                    }

                    await _context.Receipts.AddAsync(receipt);
                    await _context.SaveChangesAsync();


                 
                }

                await transaction.CommitAsync();
                if(isNew)
                {
                    _whatsAppService.SendMessage(
                   BookingUpdate.New,
                   entity
               );

                    _whatsAppService.SendMessage(
                        BookingUpdate.BookingAmountReceived,
                        entity
                    );
                }
                return entity.Id;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        public List<BookingInfo> List()
        {
            return (
                from p in _context.Bookings
                join t in _context.Townships on p.TownshipId equals t.Id
                join s in _context.Plots on p.PlotId equals s.Id
                join bs in _context.BookingStatusTypes on p.Status equals bs.Id
                join ft in _context.FileTimelines
                on new { StatusId = p.Status, WorkflowTypeId = p.WorkflowTypeId }
                equals new { ft.StatusId, ft.WorkflowTypeId }
                into ftJoin
                from ft in ftJoin.DefaultIfEmpty()


                let elapsedDays = EF.Functions.DateDiffDay(
                    p.LastStatusChangedOn,
                    DateTime.Now
                )

                select new BookingInfo
                {
                    Id = p.Id,
                    TownshipName = t.Name,
                    PlotNo = s.PlotNo,
                    PlotSize = s.PlotSize,
                    BookingDate = p.BookingDate,
                    ClientName = p.ClientName,
                    ClientEmail = p.ClientEmail,
                    ContactNo = p.ClientContactNo,
                    ClientAddress = p.ClientAddress,
                    AssociateName = p.AssociateName,
                    AssociateReraNo = p.AssociateReraNo,
                    AssociateContactNo = p.AssociateContactNo,
                    LeaderName = p.LeaderName,
                    LeaderContactNo = p.LeaderContactNo,
                    RelationType = p.RelationType,
                    RelationName = p.RelationName,
                    DDClearedOn = p.DDClearedOn,
                    DraftPreparedOn = p.DraftPreparedOn,
                    TownshipId = p.TownshipId,
                    ChequeFilePath = p.ChequeFilePath,
                    IsDDSubmittedToBank = p.IsDDSubmittedToBank,
                    WorkflowTypeId = p.WorkflowTypeId,
                    DDReceivedFromBankOn = p.DDReceivedFromBankOn,
                    CurrentStage = p.CurrentStage,
                    IsDDReceivedFromBank = p.IsDDReceivedFromBank,
                    PaymentMode = p.PaymentMode,
                    JDAPattaGivenToBankOn = p.JDAPattaGivenToBankOn,
                    IsJDAPattaGivenToBank = p.IsJDAPattaGivenToBank,
                    Amount_2 = p.Amount_2,
                    BankDDPath = p.BankDDPath,
                    TransNo = p.TransNo,
                    BankName = p.BankName,
                    BranchName = p.BranchName,
                    CompletionDate = p.CompletionDate,
                    DateOfLogin = p.DateOfLogin,
                    DateOfTransfer = p.DateOfTransfer,
                    DDAmount = p.DDAmount,
                    DDNo = p.DDNo,
                    DDNotes = p.DDNotes,
                    DDUpdateNotes = p.DDUpdateNotes,
                    DokitSigingNotes = p.DokitSigingNotes,
                    DokitSignDate = p.DokitSignDate,
                    IsDokitSigned = p.IsDokitSigned,
                    IsJDAFileSigned = p.IsJDAFileSigned,
                    JDAFileSignDate = p.JDAFileSignDate,
                    DraftGivenToBankOn = p.DraftGivenToBankOn,
                    IsCompletedOnAllSides = p.IsCompletedOnAllSides,
                    IsDraftGivenToBank = p.IsDraftGivenToBank,
                    IsDraftPrepared = p.IsDraftPrepared,
                    IsLoanSanctioned = p.IsLoanSanctioned,
                    LoanSanctionDate = p.LoanSanctionDate,
                    LoanSanctionNotes = p.LoanSanctionNotes,
                    MarkFileCheckNotes = p.MarkFileCheckNotes,
                    OriginalATTPath = p.OriginalATTPath,
                    OriginalATTNotes = p.OriginalATTNotes,
                    LoginRefNo = p.LoginRefNo,
                    Notes_3 = p.Notes_3,
                    Notes_4 = p.Notes_4,
                    IsPaymentVerified = p.IsPaymentVerified,
                    Notes_2 = p.Notes_2,
                    IsJDAPattaApplied = p.IsJDAPattaApplied,
                    IsJDAPattaRegistered = p.IsJDAPattaRegistered,
                    JDAPattaAppliedOn = p.JDAPattaAppliedOn,
                    JDAPattaNotes = p.JDAPattaNotes,
                    JDAPattaRegisteredOn = p.JDAPattaRegisteredOn,
                    StatusId = p.Status,
                    Status = bs.Name,

                    LastStatusChangedOn = p.LastStatusChangedOn,

                    AllowedDays = ft != null ? ft.Days : 0,
                    ElapsedDays = elapsedDays,
                    RemainingDays = ft != null ? ft.Days - elapsedDays : 0,

                    ProgressState =
                        ft == null ? "NO_TIMELINE" :
                        elapsedDays < ft.Days
                            ? "IN_PROGRESS"
                            : elapsedDays == ft.Days
                                ? "ON_TIME"
                                : "DELAYED",

                    ProgressColor =
                        ft == null ? "gray" :
                        elapsedDays < ft.Days
                            ? "orange"
                            : elapsedDays == ft.Days
                                ? "green"
                                : "red"
                }
            ).ToList();
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

            //Payment details received
            _whatsAppService.SendMessage(
                BookingUpdate.PaymentReceived,
                entity
            );

            // Payment confirmed (only if verified)
            if (request.IsPaymentVerified == true)
            {
                _whatsAppService.SendMessage(
                    BookingUpdate.PaymentConfirmed,
                    entity
                );
            }

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
            entity.Status = 13; //Bank Login Done
            _context.SaveChanges();

            if(!string.IsNullOrEmpty(request.LoginRefNo))
            {
                _whatsAppService.SendMessage(BookingUpdate.BankLoginDone, entity);
            }
            

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
            entity.Status = 14; //Loan Sanction Done
            _context.SaveChanges();

            if (request.IsLoanSanctioned == true)
            {
                _whatsAppService.SendMessage(BookingUpdate.LoanSanctioned, entity);
            }

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
            entity.Status = 15; //File Marked Completed
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
            entity.Status = 16; //Dokit Signed
            _context.SaveChanges();
            if (request.IsDokitSigned == true)
            {
                _whatsAppService.SendMessage(
                    BookingUpdate.DokitSigned,
                    entity
                );
            }
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

            if (request.IsJDAPattaApplied == true)
            {
                _whatsAppService.SendMessage(BookingUpdate.SentForJDAPatta, entity);
            }

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

            if (request.IsDDSubmittedToBank == true)
            {
                _whatsAppService.SendMessage(BookingUpdate.BankDDReceived, entity);
            }

            return true;
        }

        public List<BookingProgressModel> GetBookingProgress(int bookingId)
        {
            List<BookingProgressModel> progressModels= new List<BookingProgressModel>();
            var entity = _context.Bookings.Find(bookingId);
            //1. Booking Created
            progressModels.Add(new BookingProgressModel
            {
               ProgressDate = entity.BookingDate,
                ProgressDetails = "Booking Created"
            });

            //2. Booking Confirmed
            if (entity.DateOfTransfer != null)
            {
                progressModels.Add(new BookingProgressModel
                {
                    ProgressDate = entity.DateOfTransfer.GetValueOrDefault(),
                    ProgressDetails = "Booking Confirmed",
                    DaysFromBooking = entity. DateOfTransfer.GetValueOrDefault().Subtract(entity.BookingDate).Days
                });
            }

            //3. Loan Document Upload
            if (entity.IsReqDocsUploaded == true)
            {
                progressModels.Add(new BookingProgressModel
                {
                    ProgressDate = entity.ReqDocsUploadedOn.GetValueOrDefault(),
                    ProgressDetails = "Loan Document Uploaded",
                    DaysFromBooking = entity.ReqDocsUploadedOn.GetValueOrDefault().Subtract(entity.BookingDate).Days
                });
            }            //4. Agreement Draft Created
            if (entity.IsDraftPrepared.GetValueOrDefault())
            {
               
                progressModels.Add(new BookingProgressModel
                {
                    ProgressDate = entity.DraftPreparedOn.GetValueOrDefault(),
                    ProgressDetails = "Draft Prepared",
                    DaysFromBooking = entity.DraftPreparedOn.GetValueOrDefault().Subtract(entity.BookingDate).Days
                });
            }

            //4.1. Agreement Draft Given to Bank
            if (entity.IsDraftGivenToBank.GetValueOrDefault())
            {
                progressModels.Add(new BookingProgressModel
                {
                    ProgressDate = entity.DraftGivenToBankOn.GetValueOrDefault(),
                    ProgressDetails = "Draft Given To Bank",
                    DaysFromBooking = entity.DraftGivenToBankOn.GetValueOrDefault().Subtract(entity.BookingDate).Days
                });
            }

            //5. File Login
            if (entity.DateOfLogin!=null)
            {
                progressModels.Add(new BookingProgressModel
                {
                    ProgressDate = entity.DateOfLogin.GetValueOrDefault(),
                    ProgressDetails = "File Login",
                    DaysFromBooking = entity.DateOfLogin.GetValueOrDefault().Subtract(entity.BookingDate).Days
                });
            }
            //6. Loan Sanction
            if (entity.IsLoanSanctioned.GetValueOrDefault() )
            {
                progressModels.Add(new BookingProgressModel
                {
                    ProgressDate = entity.LoanSanctionDate.GetValueOrDefault(),
                    ProgressDetails = "Loan Sanctioned",
                    DaysFromBooking = entity.LoanSanctionDate.GetValueOrDefault().Subtract(entity.BookingDate).Days
                });
            }
            //7. Mark File Check & Completed
            if (entity.IsCompletedOnAllSides.GetValueOrDefault())
            {
                progressModels.Add(new BookingProgressModel
                {
                    ProgressDate = entity.CompletionDate.GetValueOrDefault(),
                    ProgressDetails = "File mark completed",
                    DaysFromBooking = entity.CompletionDate.GetValueOrDefault().Subtract(entity.BookingDate).Days
                });
            }
            //8. Original ATT 
            //if (entity.IsCompletedOnAllSides.GetValueOrDefault())
            //{
            //    progressModels.Add(new BookingProgressModel
            //    {
            //        ProgressDate = entity.CompletionDate.GetValueOrDefault(),
            //        ProgressDetails = "File mark completed",
            //        DaysFromBooking = entity.CompletionDate.GetValueOrDefault().Subtract(entity.BookingDate).Days
            //    });
            //}
            //9. Dokit Sign
            if (entity.IsDokitSigned.GetValueOrDefault())
            {
                progressModels.Add(new BookingProgressModel
                {
                    ProgressDate = entity.DokitSignDate.GetValueOrDefault(),
                    ProgressDetails = "Dokit Signed",
                    DaysFromBooking = entity.DokitSignDate.GetValueOrDefault().Subtract(entity.BookingDate).Days
                });
            }
            //9.1 JDA File Sign
            if (entity.IsJDAFileSigned.GetValueOrDefault())
            {
                progressModels.Add(new BookingProgressModel
                {
                    ProgressDate = entity.JDAFileSignDate.GetValueOrDefault(),
                    ProgressDetails = "JDA File Signed",
                    DaysFromBooking = entity.JDAFileSignDate.GetValueOrDefault().Subtract(entity.BookingDate).Days
                });
            }
            //10 JDA Patta Applied
            if (entity.IsJDAPattaApplied.GetValueOrDefault())
            {
                progressModels.Add(new BookingProgressModel
                {
                    ProgressDate = entity.JDAPattaAppliedOn.GetValueOrDefault(),
                    ProgressDetails = "JDA Patta Applied",
                    DaysFromBooking = entity.JDAPattaAppliedOn.GetValueOrDefault().Subtract(entity.BookingDate).Days
                });
            }
            //10.1 JDA Patta Registered
            if (entity.IsJDAPattaRegistered.GetValueOrDefault())
            {
                progressModels.Add(new BookingProgressModel
                {
                    ProgressDate = entity.JDAPattaRegisteredOn.GetValueOrDefault(),
                    ProgressDetails = "JDA Patta Registered",
                    DaysFromBooking = entity.JDAPattaRegisteredOn.GetValueOrDefault().Subtract(entity.BookingDate).Days
                });
            }
            //10.2 JDA Patta Given to Bank
            if (entity.IsJDAPattaGivenToBank.GetValueOrDefault())
            {
                progressModels.Add(new BookingProgressModel
                {
                    ProgressDate = entity.JDAPattaGivenToBankOn.GetValueOrDefault(),
                    ProgressDetails = "JDA Patta Given to Bank",
                    DaysFromBooking = entity.JDAPattaGivenToBankOn.GetValueOrDefault().Subtract(entity.BookingDate).Days
                });
            }
            //10.3  DD Received from Bank
            if (entity.IsDDReceivedFromBank.GetValueOrDefault())
            {
                progressModels.Add(new BookingProgressModel
                {
                    ProgressDate = entity.DDReceivedFromBankOn.GetValueOrDefault(),
                    ProgressDetails = "DD Received from Bank",
                    DaysFromBooking = entity.DDReceivedFromBankOn.GetValueOrDefault().Subtract(entity.BookingDate).Days
                });
            }
            ////11  DD Received from Bank
            //if (entity.IsDDSubmittedToBank.GetValueOrDefault())
            //{
            //    progressModels.Add(new BookingProgressModel
            //    {
            //        ProgressDate = entity.DDSubmittedOn.GetValueOrDefault(),
            //        ProgressDetails = "DD Received from Bank",
            //        DaysFromBooking = entity.DDReceivedFromBankOn.GetValueOrDefault().Subtract(entity.BookingDate).Days
            //    });
            //}
            //11.1  DD clreaed On
            if (entity.IsDDSubmittedToBank.GetValueOrDefault())
            {
                progressModels.Add(new BookingProgressModel
                {
                    ProgressDate = entity.DDClearedOn.GetValueOrDefault(),
                    ProgressDetails = "DD Cleared On",
                    DaysFromBooking = entity.DDClearedOn.GetValueOrDefault().Subtract(entity.BookingDate).Days
                });
            }
            return progressModels;
        }

        public BookingInfo GetById(int bookingId)
        {
             
            return (from p in _context.Bookings
                    join t in _context.Townships on p.TownshipId equals t.Id
                    join s in _context.Plots on p.PlotId equals s.Id
                    where p.Id==bookingId
                    select new BookingInfo
                    {
                        Id = p.Id,
                        TownshipName = t.Name,
                        PlotNo = s.PlotNo,
                        PlotSize = s.PlotSize     ,
                        BookingDate = p.BookingDate,
                        ClientName = p.ClientName,
                        ClientEmail = p.ClientEmail,
                        ContactNo = p.ClientContactNo,
                        ClientAddress=p.ClientAddress,
                        AssociateName = p.AssociateName,
                        AssociateReraNo = p.AssociateReraNo,
                        AssociateContactNo = p.AssociateContactNo,
                        LeaderName = p.LeaderName,
                        LeaderContactNo = p.LeaderContactNo,   
                        RelationType = p.RelationType,         
                        RelationName = p.RelationName,
                        DDClearedOn = p.DDClearedOn,
                        DraftPreparedOn = p.DraftPreparedOn,
                        TownshipId = p.TownshipId,
                       // ChequeFilePath = p.ChequeFilePath,
                        IsDDSubmittedToBank = p.IsDDSubmittedToBank,
                        WorkflowTypeId = p.WorkflowTypeId,
                        DDReceivedFromBankOn = p.DDReceivedFromBankOn,
                        CurrentStage = p.CurrentStage,
                        IsDDReceivedFromBank = p.IsDDReceivedFromBank,
                        PaymentMode = p.PaymentMode,
                        JDAPattaGivenToBankOn = p.JDAPattaGivenToBankOn,
                        IsJDAPattaGivenToBank = p.IsJDAPattaGivenToBank,
                        Amount_2 = p.Amount_2,
                        BankDDPath = p.BankDDPath,
                        TransNo = p.TransNo,
                        BankName = p.BankName,
                        BranchName = p.BranchName,
                        CompletionDate = p.CompletionDate,
                        DateOfLogin = p.DateOfLogin,
                        DateOfTransfer = p.DateOfTransfer,
                        DDAmount = p.DDAmount,
                        DDNo = p.DDNo,
                        DDNotes = p.DDNotes,
                        DDUpdateNotes = p.DDUpdateNotes,
                        DokitSigingNotes = p.DokitSigingNotes,
                        DokitSignDate = p.DokitSignDate,
                        IsDokitSigned = p.IsDokitSigned,
                        IsJDAFileSigned = p.IsJDAFileSigned,
                        JDAFileSignDate = p.JDAFileSignDate,
                        DraftGivenToBankOn = p.DraftGivenToBankOn,
                        IsCompletedOnAllSides = p.IsCompletedOnAllSides,
                        IsDraftGivenToBank = p.IsDraftGivenToBank,
                        IsDraftPrepared = p.IsDraftPrepared,
                        IsLoanSanctioned = p.IsLoanSanctioned,
                        LoanSanctionDate = p.LoanSanctionDate,
                        LoanSanctionNotes = p.LoanSanctionNotes,
                        MarkFileCheckNotes = p.MarkFileCheckNotes,
                        OriginalATTPath = p.OriginalATTPath,
                        OriginalATTNotes = p.OriginalATTNotes,
                        LoginRefNo = p.LoginRefNo,
                        Notes_3 = p.Notes_3,
                        Notes_4 = p.Notes_4,
                        IsPaymentVerified = p.IsPaymentVerified,
                        Notes_2 = p.Notes_2,
                        IsJDAPattaApplied = p.IsJDAPattaApplied,
                        IsJDAPattaRegistered = p.IsJDAPattaRegistered,
                        JDAPattaAppliedOn = p.JDAPattaAppliedOn,
                        JDAPattaNotes = p.JDAPattaNotes,
                        JDAPattaRegisteredOn = p.JDAPattaRegisteredOn ,
                        AgreementValue=p.AgreementValue,
                        PlotId = p.PlotId
                      
                    }).FirstOrDefault();
        }

        public bool ChangePlot(ChangePlotRequest request)
        {
            Booking existing = _context.Bookings.Find(request.BookingId);
            if (existing == null)
            {
                return false;
            }
            //Log the plot change
            PlotChangeLog log = new PlotChangeLog
            {
                BookingId = request.BookingId,
                NewPlotId = request.NewPlotId,
                NewAgreementValue = request.NewAgreementValue,
                OldPlotId = existing.PlotId,
                OldAgreementValue = existing.AgreementValue,
                PlotChangedBy = request.PlotChangedBy,
                PlotChangedOn = DateTime.Now
            };
            _context.PlotChangeLogs.Add(log);
            _context.SaveChanges();

            //Update the booking with new plot details
            existing.PlotId = request.NewPlotId;
            existing.AgreementValue = request.NewAgreementValue;
            _context.SaveChanges();

            return true;
        }

        public bool Cancel(BookingCancelRequest request)
        {
            Booking existing = _context.Bookings.Find(request.BookingId);
            if (existing == null)
            {
                return false;
            }
            //Log the plot change
            BookingCancelLog log = new BookingCancelLog
            {
                BookingId = request.BookingId,
                CancelledBy = request.CancelledBy,
                 CancelledOn =DateTime.Now,
                CancelReason = request.CancelReason  
                 
                 
            };
            _context.BookingCancelLogs.Add(log);
            _context.SaveChanges();

            //Update the booking with new plot details
            existing.Status = 99; // Cancelled
            existing.LastStatusChangedBy = request.CancelledBy;
            existing.LastStatusChangedOn = DateTime.Now;
            _context.SaveChanges();

            //check if amount is pending to refund
            //if any then create refund request with pending status
            decimal RefundAmount = 0;
            _receiptRepository.ListByBookingId(request.BookingId).ForEach(r =>
            {
                RefundAmount += r.Amount;
            });
            if (RefundAmount > 0)
            { 
                RefundRequest refund = new RefundRequest
                {
                    BookingId = request.BookingId,
                    Status = 1, // Pending
                    RefundAmount = RefundAmount,
                    Notes = "Refund initiated on booking cancellation",
                    LastStatusChangedBy = request.CancelledBy,
                    LastStatusChangedOn = DateTime.Now
                };
                _context.RefundRequests.Add(refund);
            }
            _context.SaveChanges();
            //Send Refund Initiated WhatsApp
            _whatsAppService.SendMessage(
                BookingUpdate.Cancelled,
                existing
            );

            Plot plot = _context.Plots.Find(existing.PlotId);
            if(plot == null)
            {
                return false;
            }
            plot.Status = 1; // Available
            _context.SaveChanges();

            return true;
        }

        public List<BookingInfo> Search(
            int? statusTypeId,
            int? townshipId,
            int? bookingType,
            string? reraNo,
            DateTime? fromDate)
        {
            var query =
                from p in _context.Bookings
                join t in _context.Townships on p.TownshipId equals t.Id
                join s in _context.Plots on p.PlotId equals s.Id
                join bs in _context.BookingStatusTypes on p.Status equals bs.Id

                // LEFT JOIN Receipts
                join r in _context.Receipts on p.Id equals r.BookingId into receiptGroup
                from r in receiptGroup.OrderByDescending(x => x.Id).Take(1).DefaultIfEmpty()

                where (townshipId == null || townshipId == 0 || p.TownshipId == townshipId)
                && (bookingType == null || bookingType == 0 || p.WorkflowTypeId == bookingType)
                && (statusTypeId == null || statusTypeId == 0 || p.Status == statusTypeId)
                && (string.IsNullOrEmpty(reraNo) || p.AssociateReraNo == reraNo)
                && (fromDate == null || p.BookingDate >= fromDate)

                select new BookingInfo
                {
                    Id = p.Id,
                    TownshipName = t.Name,
                    PlotNo = s.PlotNo,
                    PlotSize = s.PlotSize,
                    BookingDate = p.BookingDate,
                    ClientName = p.ClientName,
                    ClientEmail = p.ClientEmail,
                    ContactNo = p.ClientContactNo,
                    ClientAddress = p.ClientAddress,
                    AssociateName = p.AssociateName,
                    AssociateReraNo = p.AssociateReraNo,
                    AssociateContactNo = p.AssociateContactNo,
                    LeaderName = p.LeaderName,
                    RelationName = p.RelationName,
                    RelationType = p.RelationType,
                    AgreementValue = p.AgreementValue,
                    TotalAgreementValue = p.TotalAgreementValue,
                    BankName = r != null ? r.BankName : null,

                    Status = bs.Name
                };

            return query.ToList();
        }

        public bool SendToDraft(SendToDraftRequest request)
        {
            DraftRequest draftRequest = new DraftRequest(); 
            draftRequest.BookingId = request.BookingId;
            draftRequest.RequestedBy = request.UserId;
            draftRequest.Notes=request.Notes;
            draftRequest.RequestedDate = DateTime.Now;
            draftRequest.Status = 1;
            draftRequest.IsOriginalAgreement=request.IsOriginalAgreement;
            draftRequest.ApplicantName = request.ApplicantName;
            draftRequest.RelativeName = request.RelativeName;
            draftRequest.RelationType = request.RelationType;
            draftRequest.Address = request.Address;
            draftRequest.ContactNo = request.ContactNo;

            _context.DraftRequests.Add(draftRequest);
            _context.SaveChanges();

            var booking=  _context.Bookings.FirstOrDefault(p => p.Id == request.BookingId) ;
            booking.Status = 8; // On Hold for Draft Preparation
            _context.SaveChanges();

            _whatsAppService.SendMessage(BookingUpdate.SentToDraft,booking);
            return true;
        }
        public List<DraftRequestInfo> GetDraftRequests()
        {
            var q = from p in _context.DraftRequests                 
                    join b in _context.Bookings on p.BookingId equals b.Id
                    join pl in _context.Plots on b.PlotId equals pl.Id
                    join u in _context.Users on p.RequestedBy equals u.Id
                    where p.Status == 1 // Pending
                    select new DraftRequestInfo
                    {
                        Id = p.Id,                       
                        Status = p.Status,
                        Notes = p.Notes,
                        RequestedOn = p.RequestedDate,
                        RequestedByName = u.UserName,
                        BookingId = b.Id,
                        CustomerName = b.ClientName,
                        PlotNo = pl.PlotNo,
                        ApplicantName = p.ApplicantName,
                        RelativeName = p.RelativeName,
                        RelationType = p.RelationType,
                        ContactNo =p.ContactNo,
                        Address = p.Address,
                        Amount = b.TotalAgreementValue
                    };
            return q.ToList();
        }

        public bool MarkDraftComplete(MarkDraftCompleteRequest request)
        {
            DraftRequest draftRequest = _context.DraftRequests.FirstOrDefault(p => p.Id == request.Id);

            draftRequest.Status = 2; //Completed
            _context.SaveChanges();

            var booking = _context.Bookings.FirstOrDefault(p => p.Id == draftRequest.BookingId);
            booking.Status = 9; //  Draft Prepared
            _context.SaveChanges();

            return true;
        }

        public bool SendForAllotmentLetter(SendForAllotmentLetterRequestModel request)
        {
            AllotmentLetterRequest allotmentLetterRequest = new AllotmentLetterRequest();
            allotmentLetterRequest.BookingId = request.BookingId;
            allotmentLetterRequest.RequestedBy = request.UserId;
            allotmentLetterRequest.Notes = request.Notes;
            allotmentLetterRequest.RequestedDate = DateTime.Now;
            allotmentLetterRequest.ApplicantName = request.ApplicantName;
            allotmentLetterRequest.RelativeName = request.RelativeName;
            allotmentLetterRequest.RelationType = request.RelationType;
            allotmentLetterRequest.Address = request.Address;
            allotmentLetterRequest.ContactNo = request.ContactNo;

            allotmentLetterRequest.Status = 1;
          

            _context.AllotmentLetterRequests.Add(allotmentLetterRequest);
            _context.SaveChanges();

            var booking = _context.Bookings.FirstOrDefault(p => p.Id == request.BookingId);
            booking.Status = 10; // On Hold for Alloment Letter Preparation
            _context.SaveChanges();

            _whatsAppService.SendMessage(BookingUpdate.SentToAllotmentLetter, booking);

            return true;
        }

        public List<AllotmentLetterRequestInfo> GetAllotmentLetterRequests()
        {
            var q = from p in _context.AllotmentLetterRequests
                    join b in _context.Bookings on p.BookingId equals b.Id
                    join pl in _context.Plots on b.PlotId equals pl.Id
                    join u in _context.Users on p.RequestedBy equals u.Id
                    where p.Status == 1 // Pending
                    select new AllotmentLetterRequestInfo
                    {
                        Id = p.Id,
                        Status = p.Status,
                        Notes = p.Notes,
                        RequestedOn = p.RequestedDate,
                        RequestedByName = u.UserName,
                        BookingId = b.Id,
                        CustomerName = b.ClientName,
                        PlotNo = pl.PlotNo,
                        Amount = b.TotalAgreementValue,
                        ApplicantName = p.ApplicantName,
                        RelativeName = p.RelativeName,
                        RelationType = p.RelationType,
                        ContactNo = p.ContactNo,
                        Address = p.Address,
                    };
            return q.ToList();
        }

        public bool MarkAllotmentLetterComplete(MarkAllotmentLetterCompleteRequest request)
        {
            AllotmentLetterRequest allotmentLetterRequest = _context.AllotmentLetterRequests.FirstOrDefault(p => p.Id == request.Id);

            allotmentLetterRequest.Status = 2; //Completed
            _context.SaveChanges();

            var booking = _context.Bookings.FirstOrDefault(p => p.Id == allotmentLetterRequest.BookingId);
            booking.Status = 11; //  Allotment Letter prepared
            _context.SaveChanges();
            _whatsAppService.SendMessage(BookingUpdate.AllotmentLetterReceived,booking);
            return true;
        }

        public List<DocumentModel> GetCheckList(int bookingId)
        {
            List<DocumentModel> model = new List<DocumentModel>();
            //Required Documents for a Booking type
            var booking = this.GetById(bookingId);
            var docTypes = _documentRepository.GetDocTypesByWorkflow(booking.WorkflowTypeId.GetValueOrDefault());
            return model;  

        }
    }
}
