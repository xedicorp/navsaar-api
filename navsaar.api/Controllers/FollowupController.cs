using Microsoft.AspNetCore.Mvc;
using navsaar.api.Repositories;
using navsaar.api.Repositories.Followups;
using navsaar.api.ViewModels;
using navsaar.api.ViewModels.Followup;

namespace navsaar.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FollowupController : ControllerBase
    {
        IFollowupRepository _repository; 
        public FollowupController( IFollowupRepository repository)
        {            
            _repository = repository;
        }

        [HttpGet]
        [Route("GetByBookingId")]
        public IEnumerable<FollowupInfo> List(int bookingId)
        {
            return _repository.ListByBookingId(bookingId);
        }
        [HttpGet]
        [Route("GetByDay")]
        public IEnumerable<FollowupInfo> GetByDay(string day)
        {
            return _repository.GetByDay(day);
        }
        [HttpPost]
        [Route("Save")]
        public int Save(CreateEditFollowupRequest request)
        {
          return  _repository.Save(request);
        }
    }
}
