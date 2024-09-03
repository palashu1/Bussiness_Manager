using System.ComponentModel.DataAnnotations;

namespace Bussiness_Manager.Models
{
    public class loginModel
    {
        [Required(ErrorMessage = "Mobile no is required.")]
        public string phone {  get; set; }

        [Required(ErrorMessage = "Mobile number is required.")]
        [Phone(ErrorMessage = "Invalid Mobile Number.")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Mobile number must be 10 digits.")]
        public string password { get; set; }
    }
}
