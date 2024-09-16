using System.ComponentModel.DataAnnotations;

namespace Bussiness_Manager.Models
{
    public class loginModel
    {
        [Required(ErrorMessage = "Enter Mobile no")]
        [Phone(ErrorMessage = "Invalid Mobile Number.")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Mobile number must be 10 digits.")]
        public string phone {  get; set; }

        [Required(ErrorMessage = "Enter Password")]
        public string password { get; set; }
    }
}
