using navsaar.api.Infrastructure;
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
    }
}
