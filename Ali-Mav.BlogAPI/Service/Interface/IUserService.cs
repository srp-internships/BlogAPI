using Ali_Mav.BlogAPI.Models;
using Ali_Mav.BlogAPI.Models.Response;

namespace Ali_Mav.BlogAPI.Service.Interface
{
    public interface IUserService
    {
        Task<BaseResponse<List<User>>> CreateAll();
        Task<BaseResponse<User>> GetById(int userId);
        Task<BaseResponse<User>> Search(string word);
    }
}
