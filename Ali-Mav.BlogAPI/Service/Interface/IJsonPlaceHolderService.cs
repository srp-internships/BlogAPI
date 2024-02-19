using Ali_Mav.BlogAPI.Models;
using Ali_Mav.BlogAPI.Models.DTO;

namespace Ali_Mav.BlogAPI.Service.Interface
{
    public interface IJsonPlaceHolderService
    {
        Task<List<PostCreateDto>> FetchPost();
        Task<List<UserViewModel>> FetchUser();
    }
}
