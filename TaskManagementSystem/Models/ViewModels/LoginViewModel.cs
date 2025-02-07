using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(50, ErrorMessage = "The {0} must be between {2} and {1} characters long.", MinimumLength = 3)]
        [MaxLength(50, ErrorMessage = "The {0} must not exceed {1} characters.")]
        [Display(Name = "Username")]
        public string Username { get; set; }


        [Required(ErrorMessage = "Password is required.")]
        [StringLength(50, ErrorMessage = "The {0} must be between {2} and {1} characters long.", MinimumLength = 3)]
        [MaxLength(50, ErrorMessage = "The {0} must not exceed {1} characters.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}
