using BusinessLogic.Models;
using BusinessLogic.IRepositories;
using BusinessLogic.Exceptions;

namespace DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly BGEContext _dbcontext;

        public UserRepository(BGEContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public void Add(User elem)
        {
            try
            {
                _dbcontext.Users.Add(elem);
                _dbcontext.SaveChanges();
            }
            catch
            {
                throw new AddUserException();
            }
        }

        public List<User> GetAll()
        {
            return _dbcontext.Users.ToList();
        }

        public User? GetByID(long id)
        {
            return _dbcontext.Users.Find(id);
        }

        public void Update(User elem)
        {
            try
            {
                _dbcontext.Users.Update(elem);
                _dbcontext.SaveChanges();
            }
            catch
            {
                throw new UpdateUserException();
            }
        }

        public void Delete(User elem)
        {
            var tmp = _dbcontext.Users.Find(elem.ID);

            if (tmp is null)
                throw new NotExistsUserException();

            try
            {
                _dbcontext.Users.Remove(tmp);
                _dbcontext.SaveChanges();
            }
            catch
            {
                throw new DeleteUserException();
            }
        }

        public User? GetByName(string name)
        {
            return _dbcontext.Users
                   .SingleOrDefault(user => user.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
        }

        public User GetDefaultUser()
        {
            return _dbcontext.Users.Single(user => user.Name == "guest");
        }

        public List<User> GetByRole(string role)
        {
            return _dbcontext.Users
                   .Where(user => user.Roles.FirstOrDefault(r => r.RoleName == role) != null)
                   .ToList();
        }

        public List<Role> GetUserRoles(long id)
        {
            try
            {
                var roles = _dbcontext.Users.Single(user => user.ID == id).Roles;
                return roles;
            }
            catch
            {
                throw new NotExistsUserException();
            }
        }

        public void AddWithBasicRole(User user)
        {
            var newPlayer = new Player(user.Name);

            try
            {
                _dbcontext.Players.Add(newPlayer);
                var roleID = _dbcontext.Players.Single(player => player.Name == user.Name).ID;
                user.Roles.Add(new Role("player") { RoleID = roleID });
                _dbcontext.Users.Add(user);

                _dbcontext.SaveChanges();
            }
            catch
            {
                throw new AddUserException();
            }
        }
    }
}
