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
            var users = _dbcontext.Users.ToList();

            foreach (var user in users)
            {
                user.Roles = _dbcontext.Roles.Where(r => r.UserID == user.ID).ToList();
            }

            return users;
        }

        public User? GetByID(long id)
        {
            var user = _dbcontext.Users.Find(id);

            if (user == null)
                return null;

            user.Roles = _dbcontext.Roles.Where(r => r.UserID == user.ID).ToList();

            return user;
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
            return _dbcontext.Users.FirstOrDefault(user => user.Name == name);
        }

        public User GetDefaultUser()
        {
            return _dbcontext.Users.Single(user => user.Name == "guest");
        }

        public List<User> GetByRole(string role)
        {
            return _dbcontext.Users
                   .Join(_dbcontext.Roles,
                         u => u.ID,
                         r => r.UserID,
                         (u, r) => new { u, r.RoleName })
                   .Where(rl => rl.RoleName == role)
                   .Select(rl => rl.u)
                   .ToList();
        }

        public List<Role> GetUserRoles(long id)
        {
            try
            {
                var roles = _dbcontext.Roles.Where(role => role.UserID == id).ToList();
                return roles;
            }
            catch
            {
                throw new NotExistsUserException();
            }
        }

        public void AddWithBasicRole(User user)
        {
            using (var transaction = _dbcontext.Database.BeginTransaction())
            {
                var newPlayer = new Player(user.Name);

                try
                {
                    _dbcontext.Players.Add(newPlayer);
                    _dbcontext.Users.Add(user);
                    _dbcontext.SaveChanges();

                    var roleID = _dbcontext.Players.Single(player => player.Name == user.Name).ID;
                    var userID = _dbcontext.Users.Single(tmpUser => tmpUser.Name == user.Name).ID;

                    _dbcontext.Roles.Add(new Role("player") { RoleID = roleID, UserID = userID });
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
    }
}
