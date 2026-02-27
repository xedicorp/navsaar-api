using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using navsaar.api.Infrastructure;
using navsaar.api.Repositories;
using navsaar.api.ViewModels;
using Microsoft.EntityFrameworkCore;   
using navsaar.api.Infrastructure;
using DocumentFormat.OpenXml.InkML;
namespace navsaar.api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        INotificationRepository _notificationRepository;
        private readonly AppDbContext _context;

        public DashboardController(INotificationRepository notificationRepository, AppDbContext context)
        {
            _notificationRepository = notificationRepository;
            _context = context;
        }

        [HttpGet]
        [Route("Get")]
        public DashboardModel Get(int userId)
        {
            DashboardModel model = new DashboardModel();
            var list = _notificationRepository.List(0, userId);
            model.NotificationCount = new NotificationCount
            {
                HighPriority = list.Where(p => p.Priority == "High").Count(),
                MediumPriority = list.Where(p => p.Priority == "Medium").Count(),
                LowPriority = list.Where(p => p.Priority == "Low").Count()
            };
            //Add Pending Draft Request Count
            model.PendingDraftRequestCount =
                (from p in _context.DraftRequests
                 join b in _context.Bookings on p.BookingId equals b.Id
                 join pl in _context.Plots on b.PlotId equals pl.Id
                 join u in _context.Users on p.RequestedBy equals u.Id
                 where p.Status == 1
                 select p.Id).Count();

            //Pending Receipt Verification Requests
            model.PendingReceiptVerificationCount =
                (from p in _context.ReceiptVerificationRequests
                 join r in _context.Receipts on p.ReceiptId equals r.Id
                 join b in _context.Bookings on r.BookingId equals b.Id
                 join pl in _context.Plots on b.PlotId equals pl.Id
                 join u in _context.Users on p.RequestedBy equals u.Id
                 where p.Status == 1
                 select p.Id).Count();

            model.AllotmentLetterRequestCount =
               (from p in _context.AllotmentLetterRequests
                join b in _context.Bookings on p.BookingId equals b.Id
                join pl in _context.Plots on b.PlotId equals pl.Id
                join u in _context.Users on p.RequestedBy equals u.Id
                where p.Status == 1
                select p.Id).Count();

            return model;
        }
    }
}
