using Ali_Mav.BlogAPI.Data.Interfaces;
using Ali_Mav.BlogAPI.Models;
using Ali_Mav.BlogAPI.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace Ali_Mav.BlogAPI.Data.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly AppDbContext _appDbContext;
        public PostRepository(AppDbContext context)
        {
            _appDbContext = context;
        }

        public async Task AddRange(List<Post> posts)
        {

            await _appDbContext.Posts.AddRangeAsync(posts);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<List<Post>> GetPaging(int pageSize, int pagenumber)
        {
            var result = await _appDbContext.Posts.Skip(pageSize*(pagenumber-1))
                .Take(pageSize).ToListAsync();
            return result;
        }

        public async Task Create(Post entity)
        {
            _appDbContext.Posts.Add(entity);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<Post> GetById(int id)
        {
            var post = await _appDbContext.Posts.FindAsync((id));
            return post;
        }

        public async Task Delete(long id)
        {
            var post = await _appDbContext.Posts.FirstOrDefaultAsync(x=>x.Id == id);
            _appDbContext.Posts.Remove(post);
            await _appDbContext.SaveChangesAsync();
        }

        public List<Post> GetAll()
        {
            return _appDbContext.Posts.ToList();
        }

        public async Task<List<Post>> GetUserPosts(int userId)
        {
            var userPost = await _appDbContext.Posts.Where(x => x.UserId == userId).ToListAsync();
            return userPost;
        }

        public async Task<Post> Update(Post entity)
        {
            var post = await _appDbContext.Posts.FirstOrDefaultAsync(p => p.Id == entity.Id);

            post.Title = entity.Title;
            post.Body = entity.Body;
            post.User = entity.User;
            post.UserId = entity.UserId;

            await _appDbContext.SaveChangesAsync();

            return post;

        }
    }
}
