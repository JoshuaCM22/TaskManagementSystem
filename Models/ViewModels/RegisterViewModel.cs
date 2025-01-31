using System.ComponentModel.DataAnnotations;


namespace TaskManagementSystem.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage ="Please Enter Username")]
        public string Username { get; set; }
        [Required(ErrorMessage ="Please Enter Password")]
        public string Password { get; set; }
        [Required(ErrorMessage ="Please Enter Confirm Password")]
        [Compare("Password", ErrorMessage ="Password Mismatch")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage ="Please Email Address")]
        [EmailAddress(ErrorMessage ="Invalid Email Address")]
        public byte RoleId { get; set; }
    }
}