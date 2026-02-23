

using Microsoft.EntityFrameworkCore;
using navsaar.api.Infrastructure;
using navsaar.api.Models;
using navsaar.api.ViewModels;
using navsaar.api.ViewModels.Report;

namespace navsaar.api.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly AppDbContext _context;
        public ReportRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<TownshipCollectionDetail> TownshipCollectionDetailReport(int townshipId = 0)
        {
            var query = from b in _context.Bookings
                        join t in _context.Townships on b.TownshipId equals t.Id
                        join r in _context.Receipts on b.Id equals r.BookingId  
                        where t.Id == townshipId || townshipId == 0
                        select new TownshipCollectionDetail
                        {
                            TownshipName = t.Name,
                            BookingNo = b.Id,
                            CustomerName = b.ClientName,
                            CustomerContactNo = b.ClientContactNo,
                            Description = "Receipt",
                            ReceiptDate =   r.ReceiptDate  ,
                            Amount =   r.Amount  
                        };

            var township = _context.Townships.FirstOrDefault(p => p.Id == townshipId);

           var bookings = (from b in _context.Bookings
                           join t in _context.Townships on b.TownshipId equals t.Id
                           where t.Id == townshipId || townshipId == 0
                           select b).ToList();
            List<TownshipCollectionDetail> collections= query.ToList();
            //Initial Payment
          
            foreach(var booking in bookings)
            {
                collections.Add (new TownshipCollectionDetail
                {
                    TownshipName = township.Name,
                    BookingNo = booking.Id,
                    CustomerName = booking.ClientName,
                    CustomerContactNo = booking.ClientContactNo,
                    Description = "Initial Payment",
                    ReceiptDate = booking.DateOfTransfer.GetValueOrDefault(),
                    Amount = booking.Amount_2.GetValueOrDefault()
                });
                if (booking.DDClearedOn != null)
                {
                    collections.Add(new TownshipCollectionDetail
                    {
                        TownshipName = township.Name,
                        BookingNo = booking.Id,
                        CustomerName = booking.ClientName,
                        CustomerContactNo = booking.ClientContactNo,
                        Description = "Bank DD",
                        ReceiptDate = booking.DDClearedOn.GetValueOrDefault(),
                        Amount = booking.DDAmount.GetValueOrDefault()
                    });
                }
            }
            collections = collections
            .OrderByDescending(x => x.ReceiptDate)
            .ToList();

            return collections;
        }

        public List<TownshipCollectionModel> TownshipCollectionSummaryReport(int townshipId = 0)
        {

            var query =
                    from t in _context.Townships

                    join b in _context.Bookings
                        on t.Id equals b.TownshipId into tb
                    from booking in tb.DefaultIfEmpty()

                    join r in _context.Receipts
                        on booking.Id equals r.BookingId into br
                    from receipt in br.DefaultIfEmpty()

                    select new
                    {
                        TownshipId = t.Id,
                        TownshipName = t.Name,
                        ReceiptAmount = receipt != null ? receipt.Amount : 0,
                        ReceiptDate = receipt != null ? receipt.ReceiptDate : (DateTime?)null
                    };


            if (townshipId > 0)
            {
                query = query.Where(x => x.TownshipId == townshipId);
            }

            var result = query
                .GroupBy(x => new { TownshipName=x.TownshipName, TownshipId=x.TownshipId})
                .Select(g => new TownshipCollectionModel
                {
                    Id=g.Key.TownshipId,
                    TownshipName = g.Key.TownshipName,
                    TotalCollection = g.Sum(x => x.ReceiptAmount),
                    TodaysCollection = g
                        .Where(x => EF.Functions.DateDiffDay(x.ReceiptDate, DateTime.Now) == 0)
                        .Sum(x => x.ReceiptAmount)
                })
                .ToList();
            foreach (var item in result)
            {


                var bookings = _context.Bookings.Where(p => p.TownshipId == item.Id).ToList();
                if (bookings != null) { 
                var ttlInitialPayment = bookings.Sum(p => p.Amount_2.GetValueOrDefault());
                var ttlDDAmount = bookings.Where(s => s.DDClearedOn != null).Sum(p => p.DDAmount.GetValueOrDefault());
                item.TotalCollection += ttlInitialPayment + ttlDDAmount;


                var todayInitialPayment = bookings.Where(p => p.DateOfTransfer.GetValueOrDefault().Date == DateTime.Now.Date).Sum(p => p.Amount_2.GetValueOrDefault());
                var todayDDAmount = bookings.Where(p => p.DDClearedOn.GetValueOrDefault().Date == DateTime.Now.Date).Sum(p => p.DDAmount.GetValueOrDefault());

                item.TodaysCollection += todayInitialPayment + todayDDAmount;
            }

            }
            return result; 
        }

        public TownshipHealthReportModel TownshipHealthReport(int townshipId)
        {
            TownshipHealthReportModel reportModel=new TownshipHealthReportModel();
            var township= _context.Townships.FirstOrDefault(p=>p.Id == townshipId);
            if (township != null)
            {
                reportModel.TownshipName = township.Name;
                reportModel.TownshipAddress = "";

                List<Plot> plots = _context.Plots.Where(p => p.TownshipId == townshipId).ToList();

                reportModel.TotalPlotsCount = plots.Count();


                List<Plot> bookedPlots = (from p in _context.Plots
                                          join b in _context.Bookings on p.Id equals b.PlotId
                                          where p.TownshipId == townshipId
                                          select p).ToList();

                reportModel.TotalBookedPlotsCount = bookedPlots.Count();

                reportModel.TotalUnbookedPlotsCount = reportModel.TotalPlotsCount - reportModel.TotalBookedPlotsCount;

                reportModel.TotalArea = plots.Sum(p => p.PlotSize);
                reportModel.TotalBookedArea = bookedPlots.Sum(p => p.PlotSize);
                reportModel.TotalUnbookedArea = reportModel.TotalArea - reportModel.TotalBookedArea;

                List<Booking> bookings = (from b in _context.Bookings
                                          join t in _context.Townships on b.TownshipId equals t.Id
                                          where t.Id == townshipId
                                          select b).ToList();

                List<Receipt> receipts = (from b in bookings
                                          join r in _context.Receipts on b.Id equals r.BookingId
                                          select r).ToList();
                //Initial Payment
                decimal initialPayment = bookings.Sum(p => p.Amount_2).GetValueOrDefault();
                receipts.Add(new Receipt
                {
                    Amount = initialPayment
                }
                );
                //Bank DD
                decimal bankDD = bookings.Sum(p => p.DDAmount).GetValueOrDefault();
                receipts.Add(new Receipt
                {
                    Amount = bankDD
                }
                );
                reportModel.TotalBookedValue = _context.Bookings.Where(p => p.TownshipId == townshipId).Sum(p => (p.PlotSize * p.AgreementValue));
                reportModel.TotalAmountReceived = receipts.Sum(p => p.Amount);
                reportModel.TotalAmountPending = reportModel.TotalBookedValue - reportModel.TotalAmountReceived;
                reportModel.TotalAmountRefunded = 0;
               var requests = from r in _context.RefundRequests
                               join b in _context.Bookings on r.BookingId equals b.Id
                               where (b.TownshipId == townshipId && r.Status == 2)
                               select r;
                if(requests!=null && requests.Count() > 0)
                    reportModel.TotalAmountRefunded = requests.Sum(p => p.RefundAmount);

            }
            return reportModel;
        }

        public List<PlotAvailabilityInfo> PlotAvailabilityReport(int townshipId = 0, int statusId = 0)
        {
            var plotRepo = new PlotRepository(_context);
            plotRepo.ReleaseExpiredHolds();

            var query =
                from p in _context.Plots
                join t in _context.Townships on p.TownshipId equals t.Id
                join pt in _context.PlotTypes on p.PlotTypeId equals pt.Id

                // LEFT JOIN HOLD TABLE
                join h in _context.PlotHoldRequests
                    .Where(x => !x.IsDelete)
                    on p.Id equals h.PlotId into ph
                from hold in ph.DefaultIfEmpty()

                where (townshipId == 0 || p.TownshipId == townshipId)
                      && (statusId == 0 || p.Status == statusId)

                select new
                {
                    p,
                    t,
                    pt,
                    hold
                };

            var result = query
                .AsEnumerable()   // Needed for Date 
                .Select(x => new PlotAvailabilityInfo
                {
                    Id = x.p.Id,
                    TownshipId = x.p.TownshipId,
                    TownshipName = x.t.Name,

                    PlotNo = x.p.PlotNo,
                    PlotSize = x.p.PlotSize,
                    SaleableSize = x.p.SaleableSize,
                    PlotSizeInSqrmtr = x.p.PlotSizeInSqrmtr,

                    PlotTypeId = x.p.PlotTypeId,
                    PlotTypeName = x.pt.Name,

                    Status =
                        x.p.Status == 1 ? "Available" :
                        x.p.Status == 2 ? "Booked" :
                        x.p.Status == 3 && x.hold != null
                            ?  $"Hold (  {x.hold.HoldDateTime.AddHours(24):dd MMM yyyy hh:mm tt})"
                        : x.p.Status == 9 ? "Not for Sale"
                        : "Unknown"
                })
                .OrderBy(x => x.PlotNo)
                .ToList();

            return result;
        }
    }
}
