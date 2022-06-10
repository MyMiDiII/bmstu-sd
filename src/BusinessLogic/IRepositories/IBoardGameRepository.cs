using BusinessLogic.Models;

namespace BusinessLogic.IRepositories
{
    public interface IBoardGameRepository : IRepository<BoardGame>
    {
        List<BoardGame> GetByTitle(string title); 
        List<BoardGame> GetByProducer(string producer);
        List<BoardGame> GetByYear(uint year);
        List<BoardGame> GetByAge(uint age);
        List<BoardGame> GetByPlayerNum(uint num);
        List<BoardGame> GetByDuration(uint duration);
    }
}
