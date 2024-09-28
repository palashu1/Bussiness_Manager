using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bussiness_Manager.Models
{
    public class Members
    {
        [Key]
        public int memberId { get; set; }

        [Column(TypeName ="nvarchar(50)")]
        [Required(ErrorMessage = "First name is required.")]
        public string firstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")] 
        [Column(TypeName ="nvarchar(50)")]
        public string lastName { get; set; }

        [Column(TypeName = "nvarchar(64)")]
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string email { get; set; }
        [Column(TypeName = "nvarchar(15)")]

        [Required(ErrorMessage = "Mobile number is required.")]
        [Phone(ErrorMessage = "Invalid Mobile Number.")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Mobile number must be 10 digits.")]
        public string phone { get; set; }
        [Column(TypeName = "nvarchar(100)")]

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public string password { get; set; }

        //[NotMapped]
        //[Required(ErrorMessage = "Confirm Password is required.")]
        //[DataType(DataType.Password)]
        //[Compare("password", ErrorMessage = "The password and confirmation password do not match.")]
        //public string confirmPassword { get; set; }

        [Column(TypeName = "nvarchar(2)")]
        public string? dstatus {  get; set; }
        [Column(TypeName = "DateTime")]
        public DateTime? createdOn { get; set; }
        [Column(TypeName = "DateTime")]
        public DateTime? updatedOn { get; set; }
        public ICollection<ShopDetail>? ShopDetails { get; set; }
        public ICollection<Product>? Products { get; set; }
        public ICollection<Customer>? Customers { get; set; }
        public ICollection<saleInvoice>? saleInvoices { get; set; }
        public ICollection<Transactions>? Transactions { get; set; }

    }
}
