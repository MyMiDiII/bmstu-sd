using BusinessLogic.Models;
using BusinessLogic.IRepositories;
using BusinessLogic.Exceptions;

namespace DataAccess.Repositories
{
    public class OrganizerRepository : IOrganizerRepository
    {
        private readonly BGEContext _dbcontext;

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

        public void AddWithUserRole(Organizer elem, long userID)
        {
            using (var transaction = _dbcontext.Database.BeginTransaction())
            {
                try
                {
                    _dbcontext.Organizers.Add(elem);
                    _dbcontext.SaveChanges();

                    var orgID = _dbcontext.Organizers.Single(tmpOrg => tmpOrg.Name == elem.Name
                                                                    && tmpOrg.Address == elem.Address).ID;

                    _dbcontext.Roles.Add(new Role("organizer") { RoleID = orgID, UserID = userID });
                    _dbcontext.SaveChanges();

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw new AddUserException();
                }
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
                   .Where(organizer => !organizer.Deleted && organizer.Name.Contains(name))
                   .ToList();
        }

        public List<Organizer> GetByAddress(string address)
        {
            return _dbcontext.Organizers
                   .Where(organizer => !organizer.Deleted && organizer.Address.Contains(address))
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
