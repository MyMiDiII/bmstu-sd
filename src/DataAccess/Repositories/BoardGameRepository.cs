using BusinessLogic.Models;
using BusinessLogic.IRepositories;
using BusinessLogic.Exceptions;

namespace DataAccess.Repositories
{
    public class BoardGameRepository : IBoardGameRepository
    {
        private readonly BGEContext _dbcontext;

        public BoardGameRepository(BGEContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public void Add(BoardGame elem)
        {
            try
            {
                _dbcontext.Games.Add(elem);
                _dbcontext.SaveChanges();
            }
            catch
            {
                throw new AddBoardGameException();
            }
        }

        public List<BoardGame> GetAll()
        {
            return _dbcontext.Games.Where(game => !game.Deleted).ToList();
        }

        public BoardGame? GetByID(long id)
        {
            return _dbcontext.Games.Find(id);
        }

        public void Update(BoardGame elem)
        {
            try
            {
                _dbcontext.Games.Update(elem);
                _dbcontext.SaveChanges();
            }
            catch
            {
                throw new UpdateBoardGameException();
            }
        }

        public void Delete(BoardGame elem)
        {
            var tmp = _dbcontext.Games.Find(elem.ID);

            if (tmp is null)
                throw new NotExistsBoardGameException();

            if (tmp.Deleted)
                throw new AlreadyDeletedBoardGameException();

            tmp.Deleted = true;
            Update(tmp);
        }

        public List<BoardGame> GetByTitle(string title)
        {
            return _dbcontext.Games
                   .Where(game => !game.Deleted && game.Title.Contains(title, StringComparison.OrdinalIgnoreCase))
                   .ToList();
        }

        public List<BoardGame> GetByProducer(string producer)
        {
            return _dbcontext.Games
                   .Where(game => !game.Deleted
                               && game.Produser != null
                               && game.Produser.Contains(producer, StringComparison.OrdinalIgnoreCase))
                   .ToList();
        }

        public List<BoardGame> GetByYear(uint year)
        {
            return _dbcontext.Games
                   .Where(game => !game.Deleted && game.Year == year)
                   .ToList();
        }

        public List<BoardGame> GetByAge(uint minAge, uint maxAge)
        {
            if (minAge > maxAge)
                throw new WrongMinMaxBoardGameException();

            return _dbcontext.Games
                   .Where(game => !game.Deleted && game.MinAge >= minAge && game.MaxAge <= maxAge)
                   .ToList();
        }

        public List<BoardGame> GetByPlayerNum(uint minNum, uint maxNum)
        {
            if (minNum > maxNum)
                throw new WrongMinMaxBoardGameException();

            return _dbcontext.Games
                   .Where(game => !game.Deleted && game.MinPlayerNum >= minNum && game.MaxPlayerNum <= maxNum)
                   .ToList();
        }

        public List<BoardGame> GetByDuration(uint minDuration, uint maxDuration)
        {
            if (minDuration > maxDuration)
                throw new WrongMinMaxBoardGameException();

            return _dbcontext.Games
                   .Where(game => !game.Deleted && game.MinDuration >= minDuration && game.MaxDuration <= maxDuration)
                   .ToList();
        }

        public List<BoardGameEvent> GetGameEvents(long gameID)
        {
            return _dbcontext.Events
                   .Where(bgEvent => bgEvent.Games.Any(game => game.ID == gameID))
                   .ToList();
        }
    }
}
