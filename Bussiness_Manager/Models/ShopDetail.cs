using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bussiness_Manager.Models
{
    public class ShopDetail
    {
        [Key]
        public int shopId { get; set; }
        public int memberId {  get; set; }
        public Members? Members { get; set; }  // using navigation property is here for foreign key

        [Required(ErrorMessage = "Shop name is required.")]
        [Column(TypeName ="nvarchar(100)")]
        public string shopName { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [Column(TypeName = "nvarchar(500)")]
        public string shopDescription { get; set; }

        [Required(ErrorMessage = "Bussiness Type is required.")]
        [Column(TypeName = "nvarchar(100)")]
        public string bussinessType { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string? logo {  get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [Column(TypeName = "nvarchar(500)")]
        public string shopAddress { get; set; }
        [Column(TypeName = "nvarchar(2)")]
        public string? dstatus {  get; set; }
        [Column(TypeName = "DateTime")]
        public DateTime? createdOn { get; set; }
        [Column(TypeName = "DateTime")]
        public DateTime? updatedOn { get; set; }

        public ICollection<Product>? Products { get; set; }
        public ICollection<Customer>? Customers { get; set; }
        public ICollection<saleInvoice>? saleInvoices { get; set; }
        public ICollection<Transactions>? Transactions { get; set; }
    }
}
