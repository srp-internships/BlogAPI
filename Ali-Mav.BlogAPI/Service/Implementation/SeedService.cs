using Ali_Mav.BlogAPI.Models.Response;
using Ali_Mav.BlogAPI.Service.Interface;

namespace Ali_Mav.BlogAPI.Service.Implementation
{
    public class SeedService : ISeedService
    {
        private readonly IJsonPlaceHolderService _jsonPlaceHolderService;
        private readonly IUserService _userService;
        private readonly IPostService _postService;

        public SeedService(IJsonPlaceHolderService jsonPlaceHolderService, IUserService userService, IPostService postService)
        {
            _jsonPlaceHolderService = jsonPlaceHolderService;
            _userService = userService;
            _postService = postService;
        }

        public async Task<BaseResponse<string>> SeedDataBase()
        {
            var serviceResponse = new BaseResponse<string>();

            var userservice = await _userService.GetAll();

            if (!userservice.Data.Any())
            {
                var users = await _jsonPlaceHolderService.FetchUser();
                var posts = await _jsonPlaceHolderService.FetchPost();
                
                var userServiceResponse = await _userService.AddUsers(users);
                var postServiceResponse = await _postService.AddPosts(posts);

                serviceResponse.success = true;
                serviceResponse.Data = "success";
            }
            else
            {
                serviceResponse.success= false;
                serviceResponse.Description = "There is already data in the database";
            }

            return serviceResponse;
        }
    }
}
