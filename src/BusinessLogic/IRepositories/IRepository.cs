namespace BusinessLogic.IRepositories
{
    public interface IRepository<T>
    {
        void Add(T elem);
        List<T> GetAll();
        T GetByID(long id);
        void Update(T elem);
        void Delete(T elem);
    }
}
