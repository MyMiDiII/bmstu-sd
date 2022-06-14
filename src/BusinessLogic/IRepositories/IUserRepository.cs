using BusinessLogic.Models;

namespace BusinessLogic.IRepositories
{
    public interface IUserRepository : IRepository<User>
    {
        User? GetByName(string username);
        List<User> GetByRole(string role);
        bool ConnectUserToDataStore(User user);
    }
}
