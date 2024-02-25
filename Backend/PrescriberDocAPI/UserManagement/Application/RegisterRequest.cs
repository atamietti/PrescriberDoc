using System.ComponentModel.DataAnnotations;

namespace PrescriberDocAPI.UserManagement.Application
{
    public class RegisterRequest
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Fullname { get; set; } = string.Empty;

        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
        [Required, DataType(DataType.Password), Compare(nameof(Password), ErrorMessage = "Password do not mach")]
        public string ConfirmPassword { get; set; } = string.Empty;

    }
}
