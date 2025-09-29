using System.ComponentModel.DataAnnotations;

namespace DemoG03.PresentationLayer.ViewModels.Account
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "First Name Is Required")]
        [MaxLength(50, ErrorMessage = "It Cant Be More Than 50 Char")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last Name Is Required")]
        [MaxLength(50, ErrorMessage = "It Cant Be More Than 50 Char")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "User Name Is Required")]
        [MaxLength(50, ErrorMessage = "It Cant Be More Than 50 Char")]
        public string UserName { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
        public bool IsAgree { get; set; }
    }
}
