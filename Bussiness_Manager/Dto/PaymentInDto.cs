using System.ComponentModel.DataAnnotations;

namespace Bussiness_Manager.Dto
{
    public class PaymentInDto
    {
        public int memberId {  get; set; }
        public int shopId {  get; set; }
        public int customerId {  get; set; }
        public string customerName { get; set; }
        public decimal? balanceDue { get; set; }
        [Required]
        public string paymentMode {  get; set; }
        public DateTime? updatedOn { get; set; }
        public List<PaymentInDetailDto> paymentInDetailDtos { get; set; }
    }
}
