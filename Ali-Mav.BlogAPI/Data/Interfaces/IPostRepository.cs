using Ali_Mav.BlogAPI.Models;
using Ali_Mav.BlogAPI.Models.DTO;

namespace Ali_Mav.BlogAPI.Data.Interfaces
{
    public interface IPostRepository : IBaseRepository<Post>
    {
        Task<List<Post>> GetUserPosts(int userId);
        Task AddRange(List<Post> posts);

    }
}
