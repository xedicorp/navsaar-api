

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
        public IdentityRepository(AppDbContext context, IPermissionRepository permissionRepository,
             IFirebaseNotificationService firebaseNotificationService)
        {
            _context = context;
            _permissionRepository = permissionRepository;
            _firebaseNotificationService = firebaseNotificationService;
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
                    Permissions = permissions
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

            _firebaseNotificationService.Send(  request.FcmToken, otp);

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
    }
}
