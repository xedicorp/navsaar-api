using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using navsaar.api.Models;
using navsaar.api.Repositories;
using navsaar.api.ViewModels.Notifications;

namespace navsaar.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotificationController : ControllerBase
    {
        INotificationRepository _repository;

        public NotificationController(INotificationRepository repository)
        {
            _repository = repository;   
        }

        [HttpGet]
        [Route("List")]
        public IEnumerable<NotificationInfo> List(int priority, int userId)
        {
            return _repository.List(priority, userId);
        }

        [HttpGet("internal/pending-actions")]
        public IActionResult GetInternalPendingNotifications()
        {
            var result = _repository.InternalActionPendingList();
            return Ok(result);
        }
        [HttpPost]
        [Route("take-action")]
        public IActionResult TakeAction(NotificationActionRequest request)
        {
            var result = _repository.TakeAction(request);

            if (!result)
                return BadRequest("Notification not found");

            return Ok("Action updated successfully");
        }
    }
}
