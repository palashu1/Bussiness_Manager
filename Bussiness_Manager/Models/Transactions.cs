using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bussiness_Manager.Models
{
    public class Transactions
    {
        [Key]
        public int transactionId {  get; set; }
        public int memberId { get; set; }
        public Members? Members { get; set; }
        public int shopId { get; set; }
        public ShopDetail? ShopDetail { get; set; }
        public int customerId { get; set; }
        public Customer? Customer { get; set; }
        public int saleId { get; set; }
        public saleInvoice? saleInvoice { get; set; }
        [Column(TypeName ="decimal(18,3)")]
        public decimal? totalAmount { get; set; }
        [Column(TypeName = "decimal(18,3)")]
        public decimal? discount { get; set; }

        [Column(TypeName = "decimal(18,3)")]
        public decimal? netAmount { get; set; }

        [Column(TypeName = "decimal(18,3)")]
        public decimal? balanceAmount { get; set; }

        [Column(TypeName = "nvarchar(2)")]
        public string? dstatus { get; set; }
        public DateTime? createdOn { get; set; }
        public DateTime? updatedOn { get; set; }
        [Column(TypeName ="int")]
        public int? paymentNo {  get; set; }
        [Column(TypeName ="decimal(18,3)")]
        public decimal? transactionAmount { get; set; }
        [Column(TypeName ="nvarchar(50)")]
        public string? paymentMode { get; set; }
        [Column(TypeName ="nvarchar(200)")]
        public string? transactionModule {  get; set; }
    }
}
