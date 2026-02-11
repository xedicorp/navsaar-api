using Microsoft.AspNetCore.Mvc;
using navsaar.api.Models;
using navsaar.api.Repositories;
using navsaar.api.Repositories.Followups;
using navsaar.api.Repositories.Reminders;
using navsaar.api.ViewModels;
using navsaar.api.ViewModels.Followup;
using navsaar.api.ViewModels.Reminders;

namespace navsaar.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReminderController : ControllerBase
    {
        IReminderRepository _repository; 
        public ReminderController(IReminderRepository repository)
        {            
            _repository = repository;
        }

        [HttpGet]
        [Route("List")]
        public IEnumerable<Reminder> List(int userId,bool isPending=true)
        {
            return _repository.List(userId,isPending);
        }
       
        [HttpPost]
        [Route("Save")]
        public int Save(CreateEditReminderRequest request)
        {
          return  _repository.Save(request);
        }
        [HttpPost]
        [Route("Dismiss")]
        public bool Save(int reminderId)
        {
            return _repository.Dismiss(reminderId);
        }
        [HttpGet]
        [Route("GetByBookingId")]
        public IEnumerable<Reminder> GetByBookingId(int bookingId, bool isPending = true)
        {
            return _repository.GetByBookingId(bookingId, isPending);
        }

    }
}
