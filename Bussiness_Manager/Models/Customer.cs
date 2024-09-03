using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bussiness_Manager.Models
{
    public class Customer
    {
        [Key]
        public int customerId { get; set; }
        public int memberId { get; set; }
        public Members Members { get; set; }
        public int shopId { get; set; }
        public ShopDetail ShopDetail { get; set; }
        [Required(ErrorMessage = "Customer name is required")]
        [Column(TypeName = "nvarchar(100)")]
        public string name {  get; set; }
        [Required(ErrorMessage = "Address is required.")]
        [Column(TypeName = "nvarchar(500)")]
        public string address {  get; set; }

        [Required(ErrorMessage = "Mobile number is required.")]
        [Phone(ErrorMessage = "Invalid Mobile Number.")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Mobile number must be 10 digits.")]
        [Column(TypeName = "nvarchar(15)")]
        public string mobileNo { get; set; }
        [Column(TypeName = "nvarchar(2)")]
        public string dstatus {  get; set; }
        public DateTime? createdOn { get; set; }
        public DateTime? updatedOn { get; set; }

        public ICollection<saleInvoice> saleInvoices { get; set; }
    }
}
