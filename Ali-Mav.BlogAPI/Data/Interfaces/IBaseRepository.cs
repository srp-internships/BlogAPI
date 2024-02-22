namespace Ali_Mav.BlogAPI.Data.Interfaces
{
    public interface IBaseRepository<T>
    {
        Task Create(T entity);
        List<T> GetAll();
        Task<T> GetById(int id);
        Task Delete(long id);
        Task<T> Update(T entity);

    }
}
