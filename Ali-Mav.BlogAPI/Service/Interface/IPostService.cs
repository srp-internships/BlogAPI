using Ali_Mav.BlogAPI.Models;
using Ali_Mav.BlogAPI.Models.DTO;
using Ali_Mav.BlogAPI.Models.Response;

namespace Ali_Mav.BlogAPI.Service.Interface
{
    public interface IPostService
    {
        Task<BaseResponse<List<PostCreateDto>>> AddPosts (List<PostCreateDto> postDtos);
        Task<BaseResponse<List<PostGetDto>>> GetAll();
        Task<BaseResponse<List<Post>>> GetPaging(int pageSize, int pagenumber);
        Task<BaseResponse<List<Post>>> GetUserPosts(int userid);
        Task<BaseResponse<Post>> CreatePost(PostCreateDto postDto);
        Task<BaseResponse<PostGetDto>> UpdatePost(PostUpdateDto updatePost);
        Task<BaseResponse<bool>> DeletePost(int id);
    }
}
