using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using navsaar.api.Repositories;
using navsaar.api.ViewModels;
using navsaar.api.ViewModels.Identity;

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
        [HttpPost]
        [Route("Login")]
        public LoginResponse Save(ViewModels.Identity.LoginRequest request)
        {
            return _repository.Login(request);
        }
        [HttpGet]
        [Route("RoleList")]
        public IEnumerable<RoleInfo> RoleList()
        {
            return _repository.RoleList();
        }
        [HttpGet]
        [Route("RolePermissions")]
        public IEnumerable<PermissionInfo> RolePermissions(int roleId)
        {
            return _repository.RolePermissions(roleId);
        }
        [HttpGet]
        [Route("UserTownships")]
        public IEnumerable<UserTownshipInfo> UserTownships(int userId)
        {
            return _repository.UserTownships(userId);
        }
        [HttpPost]
        [Route("AssignUserTownships")]
        public bool AssignUserTownships(AssignUserTownshipsRequest request)
        {
            return _repository.AssignTownships(request);
        }
    }
}
