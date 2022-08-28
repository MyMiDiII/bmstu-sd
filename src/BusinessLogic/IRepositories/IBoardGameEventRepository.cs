using BusinessLogic.Models;

namespace BusinessLogic.IRepositories
{
    public interface IBoardGameEventRepository : IRepository<BoardGameEvent>
    {
        List<BoardGameEvent> GetByTitle(string title);
        List<BoardGameEvent> GetByDate(DateOnly date);
        List<BoardGameEvent> GetByStartTime(TimeOnly startTime);
        List<BoardGameEvent> GetByDuration(uint duration);
        List<BoardGameEvent> GetByCost(uint cost);
        List<BoardGameEvent> GetByPurchase(bool purchase);
        List<BoardGameEvent> GetByState(BoardGameEventState state);
        List<BoardGame> GetEventGames(long evendID);
        List<Player> GetEventPlayers(long eventID);
    }
}
