using BusinessLogic.Models;

namespace BusinessLogic.IRepositories
{
    public interface IProducerRepository : IRepository<Producer>
    {
        List<Producer> GetByName(string name);
        List<Producer> GetByRating(long rating);
    }
}
