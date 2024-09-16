using Bussiness_Manager.Models;
using System.ComponentModel.DataAnnotations;


namespace Bussiness_Manager.Dto
{
    public class productDto
    {
        public int productId { get; set; }
        public int memberId { get; set; }
        public int shopId { get; set; }
        [Required(ErrorMessage = "Required")]
        public string productName { get; set; }
        public string productSummary { get; set; } = string.Empty;
        [Required(ErrorMessage = "Required")]
        public decimal? qty { get; set; } = 1;
        public string hsnCode { get; set; }
        public decimal? salePrice { get; set; }
        public string? color { get; set; }
        public string? brand { get; set; }
        public decimal? size { get; set; }
        public string? productCode { get; set; }
        public string unit { get; set; }
        public string? dstatus { get; set; }
        public DateTime? createdOn { get; set; }
        public DateTime? updatedOn { get; set; }
    }
}
