using Ali_Mav.BlogAPI.Data.Interfaces;
using Ali_Mav.BlogAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Ali_Mav.BlogAPI.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _appDbContext;

        public UserRepository(AppDbContext context)
        {
            _appDbContext = context;
        }

        public async Task AddUsers(List<User> users)
        {
            await _appDbContext.Users.AddRangeAsync(users);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task Create(User entity)
        {
            _appDbContext.Users.Add(entity);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<User> GetById(int id)
        {
            var user = await _appDbContext.Users.FindAsync((id));
            return user;
        }

        public async Task Delete(long id)
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(c=>c.Id == id);
            _appDbContext.Users.Remove(user);
            await _appDbContext.SaveChangesAsync();
        }

        public List<User> GetAll()
        {
            return _appDbContext.Users.ToList();
        }

        public async Task<List<User>> SearchAsync(string key)
        {
        return  await  _appDbContext.Users.Where(u =>
                u.UserName.ToUpper().Contains(key.ToUpper()) 
                || u.FirstName.ToUpper().Contains(key.ToUpper()) 
                || u.LastName.ToUpper().Contains(key.ToUpper())).ToListAsync();
        }

        public async Task<User> Update(User entity)
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(x=>x.Id ==  entity.Id);
            user.Address = entity.Address;
            user.FirstName = entity.FirstName;
            user.UserName = entity.UserName;
            user.LastName = entity.LastName;
            user.Email = entity.Email;
            user.Phone = entity.Phone;
            user.CompanyName = entity.CompanyName;

            await _appDbContext.SaveChangesAsync();
            return user;
        }
    }
}
