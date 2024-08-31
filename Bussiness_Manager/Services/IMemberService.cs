using Bussiness_Manager.Dto;
using Bussiness_Manager.Utility;

namespace Bussiness_Manager.Services
{
    public interface IMemberService
    {
        Task<IGenericContainer<string>> createMember(membersDto Dto);
        Task<IGenericContainer<string>> login(loginDto dto);
        Task<IGenericContainer<List<shopDetailDto>>> getAllShops(int memberId);
        Task<IGenericContainer<string>> resetTokenUpdateCookie(int memberId, int shopId);
    }
}
