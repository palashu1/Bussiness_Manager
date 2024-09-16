using Bussiness_Manager.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bussiness_Manager.Dto
{
    public class saleInvoiceDto
    {
        public int saleId { get; set; }
        public int memberId { get; set; }
        public int shopId { get; set; }
        public int customerId { get; set; }
        [Required(ErrorMessage = "Required")]
        public string saleInvoiceNo { get; set; }
        public decimal? netAmount { get; set; }
        public decimal? discount { get; set; }
        public decimal? totalAmount { get; set; }
        public decimal? balanceAmount { get; set; }
        [Required(ErrorMessage = "Required")]
        public string paymentMode { get; set; }
        public decimal? paymentAmount { get; set; } = 0;
        public string dstatus { get; set; }
        public DateTime? createdOn { get; set; }
        public DateTime? updatedOn { get; set; }
    }
}
