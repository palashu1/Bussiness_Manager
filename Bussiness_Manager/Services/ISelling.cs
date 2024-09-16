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
    }
}
