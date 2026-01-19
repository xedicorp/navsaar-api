using navsaar.api.Infrastructure;
using navsaar.api.Models;
using navsaar.api.ViewModels;
using navsaar.api.ViewModels.Refund;

namespace navsaar.api.Repositories.Refunds
{
    public class RefundRepository : IRefundRepository
    {
        private readonly AppDbContext _context;
        IBookingRepository _bookingRepository;
        public RefundRepository(AppDbContext context, IBookingRepository bookingRepository)
        {
            _context = context;
            _bookingRepository = bookingRepository;
        }
        public List<RefundInfo> List()
        {
            List<RefundInfo> refunds = new List<RefundInfo>();
            var refundRequests = _context.RefundRequests.ToList();
            foreach (var refund in refundRequests)
            {
                var booking = _context.Bookings.Find(refund.BookingId);
               
                refunds.Add(new RefundInfo
                {
                    Id = refund.Id,
                    BookingId = refund.BookingId, 
                    Status = GetStatus(refund.Status),
                    BookingInfo = _bookingRepository.GetById(refund.BookingId),
                     Notes= refund.Notes,
                      RefundAmount=refund.RefundAmount
                });
            }
            return refunds;
        }
        private static string GetStatus(int status)
        { //1:Pending 2:Approved 3:Rejected 4:Processed 5:Completed
            switch (status)
            {
                case 1:
                    return "Pending";
                case 2:
                    return "Approved";
                case 3:
                    return "Rejected";
                case 4:
                    return "Processed";
                case 5:
                    return "Completed";
                default:
                    return "Pending";
            }
        }

        public bool SaveStatus(SaveRefundStatusModel model)
        {
            bool result = true;
            RefundStatusLog statusLog = new RefundStatusLog();
            statusLog.RefundRequestId = model.RefundRequestId;
            statusLog.ActualDate = model.ActualDate;
            statusLog.NewStatus = model.NewStatus;
            statusLog.StatusChangeDate = DateTime.Now;
            statusLog.StatusChangedBy = model.UserId;
            statusLog.Notes = model.Notes;
            statusLog.Amount = model.Amount;

            _context.RefundStatusLogs.Add(statusLog);
            _context.SaveChanges();

            var refundRequest = _context.RefundRequests.Find(model.RefundRequestId);
            if (refundRequest != null)
            {
                refundRequest.Status = model.NewStatus;
                _context.SaveChanges();
            }


            return result;
        }

        public List<RefundStatusInfo> StatusLogs(int refundRequestId)
        {
            List<RefundStatusInfo> logs = new List<RefundStatusInfo>();
            var refundLogs = _context.RefundStatusLogs.Where(p=>p.RefundRequestId==refundRequestId).ToList();
            if (refundLogs != null && refundLogs.Count > 0)
            {
                logs = (from log in refundLogs
                        join user in _context.Users on log.StatusChangedBy equals user.Id
                        select new RefundStatusInfo
                        {

                            ActualDate = log.ActualDate,
                            Status = GetStatus(log.NewStatus),
                            StatusChangedOn = log.StatusChangeDate,
                            StatusChangedBy = user.UserName,
                            Notes = log.Notes,
                            Amount = log.Amount
                        }).ToList();
            }
            return logs;
        }
    }
}
