namespace Bussiness_Manager.Dto
{
    public class PaymentInDetailDto
    {
        public int transactionId { get; set; }
        public int saleId { get; set; }
        public string saleInvoiceNo {  get; set; }
        public DateTime? invoiceDate { get; set; }
        public decimal? originalAmount { get; set; }
        public decimal? currentAmount { get; set; }
        public decimal? paidAmount { get; set; }
        public decimal? balanceAmount { get; set; }
        public decimal? paymentAmount { get; set; }
        public DateTime? createdOn { get; set; }
        public DateTime? updatedOn { get; set; }
    }
}
