

using Microsoft.EntityFrameworkCore;
using navsaar.api.Infrastructure;
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

        public List<TownshipCollectionModel> TownshipCollectionSummaryReport(int townshipId = 0)
        {
           
            var query = from b in _context.Bookings
                        join t in _context.Townships on b.TownshipId equals t.Id
                        join r in _context.Receipts on b.Id equals r.BookingId into br
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
                .GroupBy(x => x.TownshipName)
                .Select(g => new TownshipCollectionModel
                {
                    TownshipName = g.Key,
                    TotalCollection = g.Sum(x => x.ReceiptAmount),
                    TodaysCollection = g
                        .Where(x => EF.Functions.DateDiffDay(x.ReceiptDate, DateTime.Now) == 0)
                        .Sum(x => x.ReceiptAmount)
                })
                .ToList();

            return result; 
        }
    }
}
