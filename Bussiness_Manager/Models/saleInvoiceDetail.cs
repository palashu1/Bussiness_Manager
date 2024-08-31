using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bussiness_Manager.Models
{
    public class saleInvoiceDetail
    {
        [Key]
        public int sdId { get; set; }
        public int saleId { get; set; }
        public saleInvoice SaleInvoice { get; set; }
        public int productId { get; set; }
        public Product Product { get; set; }
        [Column(TypeName ="decimal(18,2)")]
        public decimal? netAmount { get; set; }
        [Column(TypeName ="nvarchar(2)")]
        public string dstatus {  get; set; }
        public DateTime? createdOn { get; set; }
        public DateTime? updatedOn { get; set; }
    }
}
