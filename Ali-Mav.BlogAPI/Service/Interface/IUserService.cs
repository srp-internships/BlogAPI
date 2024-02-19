using Ali_Mav.BlogAPI.Models;
using Ali_Mav.BlogAPI.Models.DTO;
using Ali_Mav.BlogAPI.Models.Response;

namespace Ali_Mav.BlogAPI.Service.Interface
{
    public interface IUserService
    {
        Task<BaseResponse<User>> GetById(int userId);
        Task<BaseResponse<List<User>>> Search(string word);
        Task<BaseResponse<List<User>>> GetAll();
        Task<BaseResponse<List<UserViewModel>>> AddUsers(List<UserViewModel> users);

    }
}
