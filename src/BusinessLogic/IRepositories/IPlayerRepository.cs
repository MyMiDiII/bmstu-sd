using BusinessLogic.Models;

namespace BusinessLogic.IRepositories
{
    public interface IPlayerRepository : IRepository<Player>
    {
        List<Player> GetByRating(uint rating);
        List<Player> GetByLeague(string league);
        List<Player> GetByEvent(long eventID);
        Player? GetByName(string name);
        void AddToEvent(BGERegistration registration);
        void DeleteFromEvent(BGERegistration registration);
        long GetRegistrationID(BGERegistration registration);
        List<BoardGameEvent> GetPlayerEvents(long playerID);
        void AddToPlayer(FavoriteBoardGame favoriteBoardGame);
        void DeleteFromPlayer(FavoriteBoardGame favoriteBoardGame);
        long GetFavoriteID(FavoriteBoardGame favoriteBoardGame);
        List<BoardGame> GetPlayerFavorites(long playerID);
    }
}
