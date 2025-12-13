using Microsoft.AspNetCore.Mvc;
using navsaar.api.Repositories;
using navsaar.api.ViewModels;

namespace navsaar.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IdentityController : ControllerBase
    {
        IIdentityRepository _repository; 
        public IdentityController(
               IIdentityRepository repository)
        {
          
            _repository = repository;
        }

        [HttpGet]
        [Route("UserList")]
        public IEnumerable<UserInfo> UserList()
        {
            return _repository.List();
        }
        //[HttpPost]
        //[Route("Save")]
        //public bool Save( TownshipCreateUpdateRequest request)
        //{
        //  return  _repository.Save(request);
        //}
    }
}
