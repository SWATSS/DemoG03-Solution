using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DemoG03.PresentationLayer.ViewModels.Users
{
    public class UsersViewModel
    {
        public string id { get; set; }
        [DisplayName("First Name")]
        public string FName { get; set; }
        [DisplayName("Last Name")]
        public string LName { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }
        //public RoleName Role { get; set; }
    }
}
