using System.Linq;

using BusinessLogic.Models;
using BusinessLogic.IRepositories;

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
            _dbcontext.Organizers.Add(elem);
            _dbcontext.SaveChanges();
        }

        public List<Organizer> GetAll()
        {
            return _dbcontext.Organizers.ToList();
        }

        public Organizer? GetByID(long id)
        {
            return _dbcontext.Organizers.Find(id);
        }

        public void Update(Organizer elem)
        {
            _dbcontext.Organizers.Update(elem);
            _dbcontext.SaveChanges();
        }

        public void Delete(Organizer elem)
        {
        }

        public List<Organizer> GetByName(string name)
        {
            return _dbcontext.Organizers
                   .Where(organizer => organizer.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
                   .ToList();
        }

        public List<Organizer> GetByAddress(string address)
        {
            return _dbcontext.Organizers
                   .Where(organizer => organizer.Address.Contains(address, StringComparison.OrdinalIgnoreCase))
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
