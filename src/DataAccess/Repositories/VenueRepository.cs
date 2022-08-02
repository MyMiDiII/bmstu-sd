using BusinessLogic.Models;
using BusinessLogic.IRepositories;
using BusinessLogic.Exceptions;

namespace DataAccess.Repositories
{
    public class VenueRepository : IVenueRepository
    {
        private readonly BGEContext _dbcontext;

        public VenueRepository(BGEContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public void Add(Venue elem)
        {
            try
            {
                _dbcontext.Venues.Add(elem);
                _dbcontext.SaveChanges();
            }
            catch
            {
                throw new AddVenueException();
            }
        }

        public List<Venue> GetAll()
        {
            return _dbcontext.Venues.Where(venue => !venue.Deleted).ToList();
        }

        public Venue? GetByID(long id)
        {
            return _dbcontext.Venues.Find(id);
        }

        public void Update(Venue elem)
        {
            try
            {
                _dbcontext.Venues.Update(elem);
                _dbcontext.SaveChanges();
            }
            catch
            {
                throw new UpdateVenueException();
            }
       }

        public void Delete(Venue elem)
        {
            var tmp = _dbcontext.Venues.Find(elem.ID);

            if (tmp is null)
                throw new NotExistsVenueException();

            if (tmp.Deleted)
                throw new AlreadyDeletedVenueException();

            tmp.Deleted = true;
            Update(tmp);
        }

        public List<Venue> GetByName(string name)
        {
            return _dbcontext.Venues
                   .Where(venue => !venue.Deleted && venue.Name.Contains(name))
                   .ToList();
        }

        public List<Venue> GetByAddress(string address)
        {
            return _dbcontext.Venues
                   .Where(venue => !venue.Deleted && venue.Address.Contains(address))
                   .ToList();
        }

        public List<Venue> GetByType(string type)
        {
            return _dbcontext.Venues
                   .Where(venue => !venue.Deleted && venue.Type.Contains(type))
                   .ToList();
        }
    }
}
