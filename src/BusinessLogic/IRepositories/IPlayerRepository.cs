using BusinessLogic.Models;

namespace BusinessLogic.IRepositories
{
    public interface IPlayerRepository : IRepository<Player>
    {
        List<Player> GetByRating(uint rating);
        List<Player> GetByLeague(string league);
        Player? GetByName(string name);
        void AddToEvent(long eventID, long playerID);
        void DeleteFromEvent(long eventID, long playerID);
        bool CheckPlayerRegistration(long eventID, long playerID);
        List<BoardGameEvent> GetPlayerEvents(long playerID);
        List<BoardGame> GetPlayerFavorites(long playerID);
    }
}
