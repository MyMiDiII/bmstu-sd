namespace BusinessLogic.IRepositories
{
    public interface IRepository<T>
    {
        long Add(T elem);
        List<T> GetAll();
        T? GetByID(long id);
        void Update(T elem);
        void Delete(T elem);
    }
}
