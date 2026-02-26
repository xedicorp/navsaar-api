using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using navsaar.api.Repositories;
using navsaar.api.ViewModels;

namespace navsaar.api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        INotificationRepository _notificationRepository;
        public DashboardController(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
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
            return model;
        }
    }
}
