using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bussiness_Manager.Models
{
    public class saleInvoice
    {
        [Key]
        public int saleId { get; set; }
        public int memberId { get; set; }
        public Members Members { get; set; }
        public int shopId { get; set; }
        public ShopDetail ShopDetail { get; set; }
        public int customerId { get; set; }
        public Customer Customer { get; set; }
        [Column(TypeName ="nvarchar(50)")]
        public string saleInvoiceNo {  get; set; }
        [Column(TypeName ="Decimal(18,2)")]
        public decimal? netAmount { get; set; }
        [Column(TypeName = "Decimal(18,2)")]
        public decimal? discount {  get; set; }
        [Column(TypeName = "Decimal(18,2)")]
        public decimal? totalAmount { get; set; }
        [Column(TypeName = "Decimal(18,2)")]
        public decimal? balanceAmount { get; set; }
        [Column(TypeName = "nvarchar(2)")]
        public string dstatus {  get; set; }
        public DateTime? createdOn { get; set; }
        public DateTime? updatedOn { get; set; }


        public ICollection<saleInvoiceDetail> saleInvoiceDetails { get; set; }
    }
}
