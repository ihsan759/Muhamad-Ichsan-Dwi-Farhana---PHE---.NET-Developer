using System.ComponentModel.DataAnnotations;

namespace Client.ViewModels.Account
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Phone Number is required")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm password is required.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        public IFormFile? Image { get; set; }
    }
}
