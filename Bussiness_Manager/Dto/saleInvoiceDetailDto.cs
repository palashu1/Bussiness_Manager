using Bussiness_Manager.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bussiness_Manager.Dto
{
    public class saleInvoiceDetailDto
    {
        public int sdId { get; set; }
        public int saleId { get; set; }
        [Required(ErrorMessage = "Required")]
        public int productId { get; set; }
        public string productName { get; set; }
        [Required(ErrorMessage = "Required")]
        public decimal? qty { get; set; }
        public decimal? price { get; set; }
        public decimal? discount { get; set; }
        public decimal? netAmount { get; set; }
        public string hsncode {  get; set; }
        public string dstatus { get; set; }
        public string saleInvoiceNo { get; set; }
        public DateTime? createdOn { get; set; }
        public DateTime? updatedOn { get; set; }
    }
}
