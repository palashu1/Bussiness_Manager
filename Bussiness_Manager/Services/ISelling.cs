using Bussiness_Manager.Dto;
using Bussiness_Manager.Utility;

namespace Bussiness_Manager.Services
{
    public interface ISelling
    {
        Task<GenericContainer<string>> createUpdateCustomer(customerDto dto);
        Task<GenericContainer<List<customerDto>>>getAllCustomers(int memberId, int shopId);
        Task<GenericContainer<string>>createUpdateProduct(productDto dto);
        Task<GenericContainer<List<productDto>>> getAllProducts(int memberId, int shopId);
        Task<GenericContainer<string>> createUpdateSellingInvoice(serviceSaleInvoiceDto dto);
        Task<GenericContainer<serviceSaleInvoiceDto>> getSalesView(int memberId, int shopId, int saleId);
        Task<GenericContainer<List<saleInvoiceListDto>>>manageSales(int memberId, int shopId);
        Task<GenericContainer<List<paymentInHistoryDto>>> paymentInHistoryList(int memberId, int shopId);
        Task<GenericContainer<paymentInHistoryDto>> paymentView(int memberId, int shopId, int transactionId);
        Task<GenericContainer<string>> deleteCustomers(int memberId, int shopId, int customerId);
        Task<GenericContainer<int>> deleteSale(int memberId, int shopId, int saleId);
        Task<GenericContainer<PaymentInDto>> paymentIn(PaymentInDto dto);
        Task<GenericContainer<int>> addShops(shopDetailDto dto);
        Task<GenericContainer<List<shopDetailDto>>> manageShops(int memberId,int shopId);
       // Task<GenericContainer<int>> DeleteShop(int memberId, int shopId);
    }
}
