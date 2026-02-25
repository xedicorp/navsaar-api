using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using navsaar.api.Models;
using navsaar.api.Repositories;

namespace navsaar.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        INotificationRepository _repository;

        public NotificationController(INotificationRepository repository)
        {
            _repository = repository;   
        }

        [HttpGet]
        [Route("List")]
        public IEnumerable<Notification> List(int priority)
        {
            return _repository.List(priority);
        }
    }
}
