using System.ComponentModel.DataAnnotations;

namespace navsaar.api.ViewModels.Identity
{
    public class CreateUserRequest
    {
        [Required]
        public string UserName { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;

        [Required]
        public int RoleId { get; set; }

        public bool IsActive { get; set; } = true;
    }

    public class UpdateUserRequest
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public int RoleId { get; set; }
        public bool IsActive { get; set; }
    }
    public class ChangePasswordRequest
    {
        public int UserId { get; set; }

        public string CurrentPassword { get; set; }

        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}