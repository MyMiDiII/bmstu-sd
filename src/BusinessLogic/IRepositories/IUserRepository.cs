using BusinessLogic.Models;

namespace BusinessLogic.IRepositories
{
    public interface IUserRepository : IRepository<User>
    {
        User GetDefauldUser();
        User? GetByName(string username);
        List<User> GetByRole(string role);
        bool ConnectUserToDataStore(User user);
        List<Role> GetUserRoles(long id);
        void AddWithBasicRole(User user);
    }
}
