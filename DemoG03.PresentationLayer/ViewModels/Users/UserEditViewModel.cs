using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DemoG03.PresentationLayer.ViewModels.Users
{
    public class UserEditViewModel
    {
        [DisplayName("First Name")]
        public string FName { get; set; }
        [DisplayName("Last Name")]
        public string LName { get; set; }
        [DisplayName("Phone Number")]
        [Phone]
        public string PhoneNumber { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
