

using navsaar.api.Infrastructure;
using navsaar.api.Models;
using navsaar.api.ViewModels;
using navsaar.api.ViewModels.Receipt;
using Twilio.TwiML.Fax;

namespace navsaar.api.Repositories
{
    public class ReceiptRepository : IReceiptRepository
    {
        private readonly AppDbContext _context;
        public ReceiptRepository(AppDbContext context)
        {
            _context = context;
        }
        public List<ReceiptInfo> List()
        {
            List<ReceiptInfo> receipts=  (from p in _context.Receipts
                    select new ReceiptInfo
                    {
                        Id = p.Id,
                        BookingId = p.BookingId,
                        Amount = p.Amount,
                        ReceiptDate = p.ReceiptDate,
                        ReceiptMethod = p.ReceiptMethod,
                        TransactionId = p.TransactionId,
                        BankName = p.BankName,
                        ChequeNo = p.ChequeNo,
                        Status = p.Status,
                        Notes = p.Notes,
                        StatusText = GetStatus(p.Status.GetValueOrDefault())
                    }).ToList();


           

            return receipts;

        }
        public string GetStatus(int statusId)
        {
            switch (statusId)
            {
                case 1:
                    return "Verification Pending";
                case 2:
                    return "Under Verification";
                case 3:
                    return "Verified";
                case 4:
                    return "Rejected";
                default:
                    return "Unknown Status";
            }
        }
        public List<ReceiptInfo> ListByBookingId(int bookingId)
        {
            List<ReceiptInfo> receipts = (from p in _context.Receipts
                    where p.BookingId == bookingId
                    select new ReceiptInfo
                    {
                        Id = p.Id,
                        BookingId = p.BookingId,
                        Amount = p.Amount,
                        ReceiptDate = p.ReceiptDate,
                        ReceiptMethod = p.ReceiptMethod,
                        TransactionId = p.TransactionId,
                        BankName = p.BankName,
                        ChequeNo = p.ChequeNo,
                        Status = p.Status,
                        Notes = p.Notes,
                        StatusText=p.Status==1? "Verification Pending" : (p.Status == 2 ? "Under Verification" : (p.Status == 3 ? "Verified" : "Rejected")),
                        ReceiptImage = string.IsNullOrEmpty(p.receiptImage)
                        ? null
                        : "https://api.navsaargroup.com/Uploads/" + p.receiptImage

                    }).ToList();

            //Initial Payment

            //var booking=  _context.Bookings.FirstOrDefault(p=> p.Id == bookingId);    

          
            //receipts.Add(new ReceiptInfo
            //{
            //    Amount = booking.Amount_2.GetValueOrDefault (),
            //    Notes = "Initial Payment",
            //     ReceiptDate = booking.DateOfTransfer ,
            //    ReceiptMethod = booking.PaymentMode.GetValueOrDefault()
            //}
            //);

            //Bank DD
            
            //receipts.Add(new ReceiptInfo
            //{
            //    Amount = booking.DDAmount.GetValueOrDefault(),
            //    Notes = "Bank DD",
            //    ReceiptDate = booking.DDClearedOn 
            //}
            //);

            return receipts;
        }

        public async Task<bool> Save(CreateUpdateReceiptModel model)
        {
            var entity = new Models.Receipt();
            if (model.Id > 0)
            {
                entity = _context.Receipts.Find(model.Id);
                if (entity == null)
                {
                    return false;
                }
            }
            entity.BookingId = model.BookingId;
            entity.Amount = model.Amount;
            entity.ReceiptDate = model.ReceiptDate;
            entity.ReceiptMethod = model.ReceiptMethod;
            entity.TransactionId = model.TransactionId;
            entity.BankName = model.BankName;
            entity.ChequeNo = model.ChequeNo;
            entity.Status = 1; // 1: Verification Pending, 2: Under Verification, 3: Verified, 4: Rejected
            entity.Notes = model.Notes;

            if (model.Id == 0)
            {
                _context.Receipts.Add(entity);
            }
            // Handle receiptImage upload 
            if (model.receiptImage != null && model.receiptImage.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var fileName = Guid.NewGuid() + "_" + model.receiptImage.FileName;
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.receiptImage.CopyToAsync(stream);
                }

                entity.receiptImage = fileName; 
            }

            _context.SaveChanges();

            this.SendVerificationRequest(new ViewModels.Receipt.VerifRequest
            {
                ReceiptId = entity.Id,
                UserId = entity.CreatedBy.GetValueOrDefault()
            });

            return true;
        }

        public bool SendVerificationRequest(VerifRequest model)
        {
            ReceiptVerificationRequest entity = new ReceiptVerificationRequest();
            entity.ReceiptId = model.ReceiptId;
            entity.RequestedBy = model.UserId;
            entity.RequestedOn = DateTime.Now;
            entity.Status = 1; // Pending
            _context.ReceiptVerificationRequests.Add(entity);
            _context.SaveChanges();

           var recept= _context.Receipts.FirstOrDefault(p => p.Id == model.ReceiptId);
            recept.Status = 2; // 2: Under Verification 1: Verification Pending, 2: Under Verification, 3: Verified, 4: Rejected
            _context.SaveChanges();
            return true;
        }

        public List<VerificationRequestInfo> VerificationRequests()
        {
             var q = from p in _context.ReceiptVerificationRequests
                     join r in _context.Receipts on p.ReceiptId equals r.Id
                     join b in _context.Bookings on r.BookingId equals b.Id
                     join pl in _context.Plots on b.PlotId equals pl.Id
                     join u in _context.Users on p.RequestedBy equals u.Id
                     where p.Status == 1 // Pending
                     select new VerificationRequestInfo
                     {
                         Id = r.Id,                       
                         Amount = r.Amount,
                         ReceiptDate = r.ReceiptDate,
                         ReceiptMethod = r.ReceiptMethod,
                         TransactionId = r.TransactionId,
                         BankName = r.BankName,
                         ChequeNo = r.ChequeNo,
                         Status = r.Status,
                         Notes = r.Notes,                        
                         RequestedOn = p.RequestedOn,
                         RequestedByName = u.UserName,
                         BookingId = b.Id,
                         CustomerName = b.ClientName, 
                          PlotNo = pl.PlotNo
                     };
            return q.ToList();
        }

        public bool Verify(VerifReceiptRequest model)
        {
            int bookingId = 0;
            bool isInitialPayment = false;
            var receipt = _context.Receipts.FirstOrDefault(p => p.Id == model.ReceiptId);
            bookingId = receipt.BookingId;
            if (receipt.Notes == "Initial Payment")
            {
                isInitialPayment = true;
            }
            else if (bookingId > 0)
            {
                var receipts = _context.Receipts.Where(p => p.BookingId == bookingId && p.Status != 4).ToList();
                if (receipts.Count == 1)
                {
                    //means intital payment
                    isInitialPayment = true;
                }
                else
                {
                    isInitialPayment = false;
                }
            }

            if (model.Status == 1) //1 Approved
            {
                receipt.Status = 3; // Verified
                _context.SaveChanges();

                if (isInitialPayment)
                {
                    _context.Bookings.Find(receipt.BookingId).Status = 2; //2: Booking Confirmed, as Initital Payment Received
                    _context.SaveChanges();
                }
            }
            else if (model.Status == 2) //2 Rejected
            {

                receipt.Status = 4; // Rejected
                _context.SaveChanges();
                if (isInitialPayment)
                {
                    _context.Bookings.Find(receipt.BookingId).Status = 1; //2: Booking Not Confirmed, as Initital Payment not received
                    _context.SaveChanges();
                }
            } 
            var verifRequest = _context.ReceiptVerificationRequests.FirstOrDefault(p => p.ReceiptId == model.ReceiptId);
            verifRequest.Status = 2; //2: Verification Done
            _context.SaveChanges();




            return true;
        }
    }
}
