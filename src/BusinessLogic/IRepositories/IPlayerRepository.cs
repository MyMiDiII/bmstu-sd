using BusinessLogic.Models;

namespace BusinessLogic.IRepositories
{
    public interface IPlayerRepository : IRepository<Player>
    {
        List<Player> GetByRating(uint rating);
        List<Player> GetByLeague(string league);
        List<Player> GetByEvent(long eventID);
        Player? GetByName(string name);
        void AddToEvent(BGEventRegistration registration);
        void DeleteFromEvent(BGEventRegistration registration);
        long GetRegistrationID(BGEventRegistration registration);
    }
}
