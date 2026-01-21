using System.ComponentModel.DataAnnotations;

namespace Votingsystem.ViewModels
{
    public class LoginViewModel
    {

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember Me?")]
        public bool RememberMe { get; set; }

        // This property is used to redirect the user back to the page 
        // they were trying to access before being asked to log in.
        public string? ReturnUrl { get; set; }
    }
}
