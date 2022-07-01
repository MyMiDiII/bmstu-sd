using System.Linq;

using BusinessLogic.Models;
using BusinessLogic.IRepositories;

namespace DataAccess.Repositories
{
    public class VenueRepository : IVenueRepository
    {
        private AppContext _dbcontext;

        public VenueRepository(AppContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public void Add(Venue elem)
        {
            _dbcontext.Venues.Add(elem);
            _dbcontext.SaveChanges();
        }

        public List<Venue> GetAll()
        {
            return _dbcontext.Venues.ToList();
        }

        public Venue? GetByID(long id)
        {
            return _dbcontext.Venues.Find(id);
        }

        public void Update(Venue elem)
        {
            _dbcontext.Venues.Update(elem);
            _dbcontext.SaveChanges();
        }

        public void Delete(Venue elem)
        {
        }

        public List<Venue> GetByName(string name)
        {
            return _dbcontext.Venues
                   .Where(venue => venue.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
                   .ToList();
        }

        public List<Venue> GetByAddress(string address)
        {
            return _dbcontext.Venues
                   .Where(venue => venue.Address.Contains(address, StringComparison.OrdinalIgnoreCase))
                   .ToList();
        }

        public List<Venue> GetByType(string type)
        {
            return _dbcontext.Venues
                   .Where(venue => venue.Type.Contains(type, StringComparison.OrdinalIgnoreCase))
                   .ToList();
        }
    }
}
