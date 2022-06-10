using BusinessLogic.Models;

namespace BusinessLogic.IRepositories
{
    public interface IPlayerRepository : IRepository<Player>
    {
        List<Player> GetByRating(uint rating);
        List<Player> GetByLeague(string league);
        Player GetByName(string name);
    }
}
