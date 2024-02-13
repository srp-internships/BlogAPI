using Ali_Mav.BlogAPI.Models;

namespace Ali_Mav.BlogAPI.Data.Interfaces
{
    public interface IPostRepository : IBaseRepository<Post>
    {
        Task<List<Post>> GetUserPosts(int userId);

    }
}
