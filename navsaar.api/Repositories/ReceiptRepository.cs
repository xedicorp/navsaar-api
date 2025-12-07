

using navsaar.api.Infrastructure;
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
            return (from p in _context.Receipts
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
