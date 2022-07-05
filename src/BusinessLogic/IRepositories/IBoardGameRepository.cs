using BusinessLogic.Models;

namespace BusinessLogic.IRepositories
{
    public interface IBoardGameRepository : IRepository<BoardGame>
    {
        List<BoardGame> GetByTitle(string title); 
        List<BoardGame> GetByProducer(string producer);
        List<BoardGame> GetByYear(uint year);
        List<BoardGame> GetByAge(uint minAge, uint maxAge);
        List<BoardGame> GetByPlayerNum(uint minNum, uint maxNum);
        List<BoardGame> GetByDuration(uint minDuration, uint maxDuration);
        List<BoardGameEvent> GetGameEvents(long gameID);
    }
}
