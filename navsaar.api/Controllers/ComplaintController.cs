using Microsoft.AspNetCore.Mvc;
using navsaar.api.Models;
using navsaar.api.Repositories;
using navsaar.api.ViewModels;

namespace navsaar.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ComplaintController : ControllerBase
    {
        IComplaintRepository _repository; 
        public ComplaintController(
               IComplaintRepository repository )
        {
       
            _repository = repository;
           
        }

        [HttpGet]
        [Route("List")]
        public IEnumerable<ComplaintInfo> List()
        {
            return _repository.List();
        }
       

    }
}
