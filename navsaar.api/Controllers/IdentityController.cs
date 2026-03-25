using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using navsaar.api.Repositories;
using navsaar.api.ViewModels;
using navsaar.api.ViewModels.Associate;
using navsaar.api.ViewModels.Identity;

namespace navsaar.api.Controllers
{
    [ApiController]
    [Authorize]
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
        [AllowAnonymous]
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
        [HttpPost]
        [Route("SaveRolePermissions")]
        public bool SaveRolePermissions(SaveRolePermissionRequest request)
        {
            return _repository.SaveRolePermissions(request);
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
        [HttpPost]
        [AllowAnonymous]
        [Route("associate-login")]
        public IActionResult AssociateLogin([FromBody] AssociateLoginRequest request)
        {
            var result = _repository.AssociateLogin(request);

            if (!result.IsSuccessful)
                return BadRequest(result);

            return Ok(result);
        }
        [HttpPost]
        [AllowAnonymous]
        [Route("SendOtp")]
        public SendOTPResponse SendOtp(SendOTPRequest request)
        {
            return _repository.SendOTP(request);
        }

        [HttpPost]
        [Route("CreateUser")]
        public IActionResult CreateUser(CreateUserRequest request)
        {
            bool result = _repository.CreateUser(request);

            if (!result)
                return BadRequest("Username already exists");

            return Ok(true);
        }
        [HttpPut]
        [Route("UpdateUser")]
        public IActionResult UpdateUser([FromBody] UpdateUserRequest request)
        {
            var result = _repository.UpdateUser(request);

            if (result == "Success")
                return Ok("User updated successfully.");

            return BadRequest(result);
        }

        [HttpPut]
        [Route("ToggleUser")]
        public IActionResult ChangeUserStatus(int userId, bool isActive)
        {
            var result = _repository.ToggleUserStatus(userId, isActive);

            if (!result)
                return NotFound("User not found.");

            return Ok(isActive ? "User activated successfully." : "User deactivated successfully.");
        }

        [HttpPut]
        [Route("ChangePassword")]
        public IActionResult ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var result = _repository.ChangePassword(request);

            if (result == "Success")
                return Ok("Password changed successfully.");

            return BadRequest(result);
        }
    }
}
