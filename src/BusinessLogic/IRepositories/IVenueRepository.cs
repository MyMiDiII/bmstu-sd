using BusinessLogic.Models;

namespace BusinessLogic.IRepositories
{
    public interface IVenueRepository : IRepository<Venue>
    {
        List<Venue> GetByName(string name);
        List<Venue> GetByAddress(string address);
        List<Venue> GetByType(string type);
    }
}
