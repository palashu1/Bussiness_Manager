using System.ComponentModel.DataAnnotations;

namespace Bussiness_Manager.Dto
{
    public class serviceSaleInvoiceDto
    {
        public int saleId { get; set; }
        public int memberId { get; set; }
        public int shopId { get; set; }
        public int customerId { get; set; }
        public string customerName { get; set; }
        public string customerAddress {  get; set; }
        public string customerMobileNo {  get; set; }
        public string shopName {  get; set; }
        public string shopAddress {  get; set; }
        public string memberMobileNo {  get; set; }
        public string saleInvoiceNo { get; set; }
        public decimal netAmount { get; set; }
        public decimal? discount { get; set; }
        public decimal? totalAmount { get; set; }
        public decimal? balanceAmount { get; set; }
        public string paymentMode { get; set; }
        public decimal? paymentAmount { get; set; } = 0;
        public string dstatus { get; set; }
        public DateTime? createdOn { get; set; }
        public DateTime? updatedOn { get; set; }
        public List<saleInvoiceDetailDto> saleInvoiceDetailDtos { get; set; }
    }
}
