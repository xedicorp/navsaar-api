

using navsaar.api.Infrastructure;
using navsaar.api.Models;
using navsaar.api.ViewModels;

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
                        Notes = p.Notes

                    }).ToList();


           

            return receipts;

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
                        Notes = p.Notes

                    }).ToList();

            //Initial Payment

            var booking=  _context.Bookings.FirstOrDefault(p=> p.Id == bookingId);    

          
            receipts.Add(new ReceiptInfo
            {
                Amount = booking.Amount_2.GetValueOrDefault (),
                Notes = "Initial Payment",
                 ReceiptDate = booking.DateOfTransfer ,
                ReceiptMethod = booking.PaymentMode.GetValueOrDefault()
            }
            );
            //Bank DD
            
            receipts.Add(new ReceiptInfo
            {
                Amount = booking.DDAmount.GetValueOrDefault(),
                Notes = "Bank DD",
                ReceiptDate = booking.DDClearedOn 
            }
            );

            return receipts;
        }

        public bool Save(CreateUpdateReceiptModel model)
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
            entity.Status = model.Status;
            entity.Notes = model.Notes;

            if (model.Id == 0)
            {
                _context.Receipts.Add(entity);
            }
            _context.SaveChanges();
            return true;
        }
    }
}
