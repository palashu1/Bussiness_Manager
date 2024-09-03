using Bussiness_Manager.Dto;
using Bussiness_Manager.Utility;

namespace Bussiness_Manager.Services
{
    public interface ISelling
    {
        Task<GenericContainer<string>> createUpdateCustomer(customerDto dto);
    }
}
