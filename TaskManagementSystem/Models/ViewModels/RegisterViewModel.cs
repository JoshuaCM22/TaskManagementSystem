using System.ComponentModel.DataAnnotations;


namespace TaskManagementSystem.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Display(Name = "Username")]
        [Required(ErrorMessage ="User is required")]
        public string Username { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage ="Password is required")]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage ="Confirm Password is required")]
        [Compare("Password", ErrorMessage ="Password Mismatch")]
        public string ConfirmPassword { get; set; }
    }
}