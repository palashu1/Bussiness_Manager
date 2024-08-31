using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bussiness_Manager.Models
{
    public class Product
    {
        [Key]
        public int productId { get; set; }
        public int memberId { get; set; }
        public Members Members { get; set; }
        public int shopId {  get; set; }
        public ShopDetail ShopDetail { get; set; }
        [Column(TypeName ="nvarchar(500)")]
        public string productName { get; set; }
        [Column(TypeName = "nvarchar(500)")]
        public string? productDescription { get; set; }=string.Empty;
        [Column(TypeName = "nvarchar(500)")]
        public string? productCategory { get; set; }=string.Empty;
        [Column(TypeName = "Decimal(18,2)")]
        public decimal? qty { get; set; }
        [Column(TypeName = "Decimal(18,2)")]
        public decimal? salePrice { get; set; }
        [Column(TypeName = "nvarchar(2)")]
        public string dstatus { get; set; }
        public DateTime? createdOn { get; set; }
        public DateTime? updatedOn { get; set; }

        public ICollection<saleInvoiceDetail> saleInvoiceDetails { get; set; }
    }
}
