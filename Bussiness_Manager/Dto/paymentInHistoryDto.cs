namespace Bussiness_Manager.Dto
{
    public class paymentInHistoryDto
    {
        public int memberId {  get; set; }
        public int shopId {  get; set; }
        public int saleId { get; set; }
        public string memberMobileNo {  get; set; }
        public string shopName {  get; set; }
        public string logo {  get; set; }
        public string saleInvoiceNo { get; set; }
        public int transactionId { get; set; }
        public DateTime? transactionDate { get; set; }
        public DateTime transactionDateView { get; set; }
        public int? customerId { get; set; }
        public string customerName { get; set; }
        public string customerMobileNo {  get; set; }
        public decimal? netAmount {  get; set; } 
        public decimal? paidAmount { get; set; }
        public string amountInWords {  get; set; }
        public decimal? balanceAmount { get; set; }
        public int? paymentNo {  get; set; }
        public string paymentMode { get; set; }
        public string transactionModule {  get; set; }
        public string dstatus {  get; set; }
        public DateTime? createdOn { get;set; }
        public DateTime? updatedOn { get; set; }
    }
}
