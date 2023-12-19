using System.ComponentModel.DataAnnotations;

namespace UserManagementAPI.Models.SignUp
{
    public class RegisterUser
    {
        [Required(ErrorMessage = "UserName is required")]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; set; }

    }
}
