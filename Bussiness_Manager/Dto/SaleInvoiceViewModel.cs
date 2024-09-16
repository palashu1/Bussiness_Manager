namespace Bussiness_Manager.Dto
{
    public class SaleInvoiceViewModel
    {
        public saleInvoiceDto SaleInvoice { get; set; }
        public List<customerDto> Customers { get; set; }
        public List<productDto> Products { get; set; }
        public List<saleInvoiceDetailDto> SaleInvoiceList { get; set; }
        public List<modeOfPaymentDto> paymentModes { get; set; }
    }
}
