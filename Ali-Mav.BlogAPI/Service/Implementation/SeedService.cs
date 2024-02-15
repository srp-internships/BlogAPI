using Ali_Mav.BlogAPI.Models.Response;
using Ali_Mav.BlogAPI.Service.Interface;

namespace Ali_Mav.BlogAPI.Service.Implementation
{
    public class SeedService : ISeedService
    {
        private readonly IUserService _userService;
        private readonly IPostService _postService;

        public SeedService(IUserService userService, IPostService postService)
        {
            _postService = postService;
            _userService = userService;
        }

        public async Task<BaseResponse<string>> SeedDataBase()
        {
            var users = await _userService.CreateAll();
            if (users.success)
            {
                var posts = await _postService.CreateAll();

                return new BaseResponse<string> { success = true };
            }
            return new BaseResponse<string> { Description = users.Description };
        }
    }
}
