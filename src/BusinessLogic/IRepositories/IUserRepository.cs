using BusinessLogic.Models;

namespace BusinessLogic.IRepositories
{
    public interface IUserRepository : IRepository<User>
    {
        User GetDefaultUser();
        User? GetByName(string username);
        List<User> GetByRole(string role);
        List<Role> GetUserRoles(long id);
        void AddWithBasicRole(User user);
    }
}
