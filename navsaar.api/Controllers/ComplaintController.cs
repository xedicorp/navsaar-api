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
        [HttpPost]
        [Route("Save")]
        public async Task<bool> Save([FromForm] CreateUpdateComplaintModel model)
        {
            return await _repository.Save(model);
        }

        [HttpGet]
        [Route("GetById/{id}")]
        public ComplaintInfo GetById(int id)
        {
            return _repository.GetById(id);
        }
        [HttpPost]
        [Route("MarkComplete/{id}")]
        public bool MarkComplete(int id)
        {
            return _repository.MarkComplete(id);
        }

    }
}
