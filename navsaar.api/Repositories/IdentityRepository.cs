

using Azure.Core;
using Microsoft.EntityFrameworkCore;
using navsaar.api.Infrastructure;
using navsaar.api.Models;
using navsaar.api.Repositories.Identity;
using navsaar.api.Services;
using navsaar.api.ViewModels;
using navsaar.api.ViewModels.Associate;
using navsaar.api.ViewModels.Identity;
using System.Security.Cryptography;

namespace navsaar.api.Repositories
{
    public class IdentityRepository : IIdentityRepository
    {
        private readonly AppDbContext _context;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IFirebaseNotificationService _firebaseNotificationService;
        private readonly ITokenService _tokenService;
        public IdentityRepository(AppDbContext context, IPermissionRepository permissionRepository,
             IFirebaseNotificationService firebaseNotificationService, ITokenService tokenService   )
        {
            _context = context;
            _permissionRepository = permissionRepository;
            _firebaseNotificationService = firebaseNotificationService;
            _tokenService = tokenService;
        }
        public List<UserInfo> List()
        {
             return( from p in _context.Users
                     join r in _context.Roles on p.RoleId equals r.Id   
                     select new UserInfo
                    {
                        Id = p.Id,
                        UserName = p.UserName,
                        IsActive = p.IsActive,
                         RoleId = p.RoleId,
                         RoleName = r.Name  
                     }).ToList();

        }
        public List<RoleInfo> RoleList()
        {
            return (from p in _context.Roles
                    
                    select new RoleInfo
                    {
                        Id = p.Id,
                        Name = p.Name
                    }).ToList();

        }
        public LoginResponse Login(LoginRequest request)
        {
          
            bool isValid = _context.Users.Any(p => p.UserName == request.UserName && p.Password == request.Password && p.IsActive);
            if (isValid)
            {
                var usr = _context.Users.FirstOrDefault(p => p.UserName == request.UserName
                        && p.Password == request.Password && p.IsActive);

                List<PermissionInfo> permissions = _permissionRepository.GetRolePermissions(usr.RoleId);
                string roleName = _context.Roles.FirstOrDefault(r => r.Id == usr.RoleId)?.Name;
                var token = _tokenService.GenerateToken(usr.Id.ToString(), roleName);
                return new LoginResponse
                {
                    IsSuccessful = true,
                    User = new UserInfo
                    {
                        Id = usr.Id,
                        IsActive = usr.IsActive,
                        RoleId = usr.RoleId,
                        RoleName = roleName,
                        UserName = usr.UserName
                    },
                    Permissions = permissions,
                    Token = token
                };
            }
            else
            {
                return new LoginResponse
                {
                    IsSuccessful = false,
                    User = null,
                    Permissions = null
                };
            }
        }

        public List<PermissionInfo> RolePermissions(int roleId)
        {
            return _permissionRepository.GetRolePermissions(roleId);

        }
       
        public List<UserTownshipInfo> UserTownships(int userId)
        {
            return _permissionRepository.GetUserTownships (userId);
        }

        public bool AssignTownships(AssignUserTownshipsRequest request)
        {
            return _permissionRepository.AssignTownships(request.UserId,request.UserTownships );
        }

        public bool SaveRolePermissions(SaveRolePermissionRequest request)
        {
            return _permissionRepository.AssignRolePermissions(request.RoleId, request.Permissions);
        }
        public AssociateLoginResponse AssociateLogin(AssociateLoginRequest request)
        {
            var associate = _context.Associates
                .FirstOrDefault(x => x.ContactNo == request.MobileNo);

            if (associate == null)
            {
                return new AssociateLoginResponse
                {
                    IsSuccessful = false,
                    Message = "Mobile number does not exist",
                    Associate = null
                };
            }

            // OTP check

            var otp = _context.MobileOtps.FirstOrDefault(p => p.MobileNo == request.MobileNo && p.Otp ==request.Otp);
            if (otp == null)
            {
                return new AssociateLoginResponse
                {
                    IsSuccessful = false,
                    Message = "Invalid OTP",
                    Associate = null
                };

            }
 

            return new AssociateLoginResponse
            {
                Token = _tokenService.GenerateToken(associate.ContactNo, "associate"),
                IsSuccessful = true,
                Message = "Login successful",
                Associate = new AssociateInfo
                {
                    Id = associate.ID,
                    FirstName = associate.FirstName,
                    ContactNo = associate.ContactNo,
                    LeaderName = associate.LeaderName,
                    LeaderContactNo = associate.LeaderContactNo,
                    ReraNo = associate.RERA,
                    IsActive = associate.IsActive,
                    IsApproved = associate.IsApproved
                }
            };
        }

        public SendOTPResponse SendOTP(SendOTPRequest request)
        {
            //Check mobile no
            var associate = _context.Associates
             .FirstOrDefault(x => x.ContactNo == request.MobileNo);

            if (associate == null)
            {
                return new SendOTPResponse
                {
                    IsSuccessful = false,
                    Message = "Mobile number does not exists in system",

                };
            }
            if (associate.IsTerminated)
            {
                return new SendOTPResponse
                {
                    IsSuccessful = false,
                    Message = "Associate is terminated,cannot send OTP"
                };
            }
            //Store fcm with Mobile No

            string otp = GenerateSecureOtp();
            //Store Otp

            var existingOtp = _context.MobileOtps.FirstOrDefault(p => p.MobileNo == request.MobileNo);
            if (existingOtp != null)
            {
                existingOtp.Otp = otp;
                existingOtp.ExpiresAt = DateTime.UtcNow.AddMinutes(15);
            }
            else
            {
                existingOtp = new MobileOtp()
                {
                    Otp = otp,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(15),
                    MobileNo = request.MobileNo,
                };
                _context.MobileOtps.Add(existingOtp);
            }
            _context.SaveChanges();

            var existing = _context.FcmTokens.FirstOrDefault(p => p.MobileNo == request.MobileNo);
            if (existing == null)
            {
                _context.FcmTokens.Add(new FcmToken
                {

                    MobileNo = request.MobileNo,
                    Token = request.FcmToken
                });
            }
            else
            {
                existing.Token = request.FcmToken;

            }
            _context.SaveChanges();

            //Send Notification
            try
            {
                _firebaseNotificationService.Send(request.FcmToken, otp);
            }
            catch (Exception ex)
            {
                File.AppendAllLines("log.txt", new List<string>  { ex.Message });
                return new SendOTPResponse
                {
                    IsSuccessful = false,
                    Message = "OTP could not sent."
                };

            }
          

            return new SendOTPResponse
            {
                IsSuccessful = true,
                Message = "OTP sent sucessfully"
            };
        }
        public static string GenerateSecureOtp()
        {
            // Get a cryptographically secure random number between 0 and 900000 (exclusive of 900000)
            int randomNumber = RandomNumberGenerator.GetInt32(0, 900000);

            // Add 100000 to ensure the result is in the range 100000 to 999999
            int sixDigitNumber = randomNumber + 100000;

            // Format as a 6-digit string, adding leading zeros if necessary (though the above logic should prevent this)
            return sixDigitNumber.ToString("D6");
        }

        public bool CreateUser(CreateUserRequest request)
        {
            // Check duplicate username
            bool exists = _context.Users.Any(u => u.UserName == request.UserName);
            if (exists)
                return false;

            var user = new User
            {
                UserName = request.UserName,
                Password = request.Password,
                RoleId = request.RoleId,
                IsActive = request.IsActive,
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return true;
        }
        public string UpdateUser(UpdateUserRequest request)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == request.Id);

            if (user == null)
                return "User not found.";

            // Check duplicate username (excluding current user)
            bool exists = _context.Users
                .Any(x => x.UserName == request.UserName && x.Id != request.Id);

            if (exists)
                return "Username already exists.";

            // Update fields (NOT password)
            user.UserName = request.UserName;
            user.RoleId = request.RoleId;
            user.IsActive = request.IsActive;
            _context.SaveChanges();

            return "Success";
        }
        public bool ToggleUserStatus(int userId, bool isActive)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == userId);

            if (user == null)
                return false;

            user.IsActive = isActive;

            _context.SaveChanges();

            return true;
        }

        public string ChangePassword(ChangePasswordRequest request)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == request.UserId);

            if (user == null)
                return "User not found.";

            // Check current password
            if (user.Password != request.CurrentPassword)
                return "Current password is incorrect.";

            // Check confirm password
            if (request.NewPassword != request.ConfirmPassword)
                return "New password and confirm password do not match.";

            // Prevent same password
            if (user.Password == request.NewPassword)
                return "New password cannot be same as current password.";

            // Update password
            user.Password = request.NewPassword;

            _context.SaveChanges();

            return "Success";
        }
    }
}
