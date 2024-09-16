namespace Bussiness_Manager.Dto
{
    public class saleInvoiceListDto
    {
        public int memberId {  get; set; }
        public int shopId {  get; set; }
        public int saleId {  get; set; }
        public int customerId { get; set; } 
        public string invoiceNo {  get; set; }
        public string customerName {  get; set; }
        public decimal? netAmount { get; set; }
        public decimal? paidAmount {  get; set; }
        public decimal? balanceAmount {  get; set; }
        public string dstatus {  get; set; }
        public DateTime? createdOn { get; set; }
        public DateTime? updatedOn { get; set; }

    }
}
