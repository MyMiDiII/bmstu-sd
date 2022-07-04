using System.Linq;

using BusinessLogic.Models;
using BusinessLogic.IRepositories;
using BusinessLogic.Exceptions;

namespace DataAccess.Repositories
{
    public class OrganizerRepository : IOrganizerRepository
    {
        private BGEContext _dbcontext;

        public OrganizerRepository(BGEContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public void Add(Organizer elem)
        {
            try
            {
                _dbcontext.Organizers.Add(elem);
                _dbcontext.SaveChanges();
            }
            catch
            {
                throw new AddOrganizerException();
            }
        }

        public List<Organizer> GetAll()
        {
            return _dbcontext.Organizers.Where(organizer => !organizer.Deleted).ToList();
        }

        public Organizer? GetByID(long id)
        {
            return _dbcontext.Organizers.Find(id);
        }

        public void Update(Organizer elem)
        {
            try
            {
                _dbcontext.Organizers.Update(elem);
                _dbcontext.SaveChanges();
            }
            catch
            {
                throw new UpdateOrganizerException();
            }
       }

        public void Delete(Organizer elem)
        {
            var tmp = _dbcontext.Organizers.Find(elem.ID);

            if (tmp is null)
                throw new NotExistsOrganizerException();

            if (tmp.Deleted)
                throw new AlreadyDeletedOrganizerException();

            tmp.Deleted = true;
            Update(tmp);
        }

        public List<Organizer> GetByName(string name)
        {
            return _dbcontext.Organizers
                   .Where(organizer => !organizer.Deleted && organizer.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
                   .ToList();
        }

        public List<Organizer> GetByAddress(string address)
        {
            return _dbcontext.Organizers
                   .Where(organizer => !organizer.Deleted && organizer.Address.Contains(address, StringComparison.OrdinalIgnoreCase))
                   .ToList();
        }
        public List<BoardGameEvent> GetOrganizerEvents(long organizerID)
        {
            return _dbcontext.Events
                   .Where(bgEvent => bgEvent.OrganizerID == organizerID)
                   .ToList();
        }
    }
}
