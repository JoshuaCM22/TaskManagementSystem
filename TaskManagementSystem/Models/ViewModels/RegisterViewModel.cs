using System.ComponentModel.DataAnnotations;


namespace TaskManagementSystem.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Display(Name = "Username")]
        [Required(ErrorMessage ="Please Enter Username")]
        public string Username { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage ="Please Enter Password")]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage ="Please Enter Confirm Password")]
        [Compare("Password", ErrorMessage ="Password Mismatch")]
        public string ConfirmPassword { get; set; }
    }
}