using System.ComponentModel.DataAnnotations;

namespace Bussiness_Manager.Models
{
    public class loginModel
    {
        [Required(ErrorMessage = "Mobile no is required.")]
        public string phone {  get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string password { get; set; }
    }
}
