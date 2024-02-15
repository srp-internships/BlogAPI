using Ali_Mav.BlogAPI.Models.Response;

namespace Ali_Mav.BlogAPI.Service.Interface
{
    public interface ISeedService
    {
        Task<BaseResponse<string>> SeedDataBase();
    }
}