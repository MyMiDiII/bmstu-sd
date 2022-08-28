using BusinessLogic.Models;
using BusinessLogic.IRepositories;
using BusinessLogic.Exceptions;

namespace DataAccess.Repositories
{
    public class BoardGameEventRepository : IBoardGameEventRepository
    {
        private readonly BGEContext _dbcontext;

        public BoardGameEventRepository(BGEContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public void Add(BoardGameEvent elem)
        {
            try
            {
                _dbcontext.Events.Add(elem);
                _dbcontext.SaveChanges();
            }
            catch
            {
                throw new AddBoardGameEventException();
            }
        }

        public List<BoardGameEvent> GetAll()
        {
            return _dbcontext.Events.Where(bgEvent => !bgEvent.Deleted).ToList();
        }

        public BoardGameEvent? GetByID(long id)
        {
            return _dbcontext.Events.Find(id);
        }

        public void Update(BoardGameEvent elem)
        {
            try
            {
                _dbcontext.Events.Update(elem);
                _dbcontext.SaveChanges();
            }
            catch
            {
                throw new UpdateBoardGameEventException();
            }
        }

        public void Delete(BoardGameEvent elem)
        {
            var tmp = _dbcontext.Events.Find(elem.ID);

            if (tmp is null)
                throw new NotExistsBoardGameEventException();

            if (tmp.Deleted)
                throw new AlreadyDeletedBoardGameEventException();

            tmp.Deleted = true;
            Update(tmp);
        }

        public List<BoardGameEvent> GetByTitle(string title)
        {
            return _dbcontext.Events
                   .Where(bgEvent => !bgEvent.Deleted && bgEvent.Title.Contains(title))
                   .ToList();
        }

        public List<BoardGameEvent> GetByDate(DateOnly date)
        {
            return _dbcontext.Events
                   .Where(bgEvent => !bgEvent.Deleted && bgEvent.Date == date)
                   .ToList();
        }

        public List<BoardGameEvent> GetByStartTime(TimeOnly time)
        {
            return _dbcontext.Events
                   .Where(bgEvent => !bgEvent.Deleted && bgEvent.StartTime == time)
                   .ToList();
        }

        public List<BoardGameEvent> GetByDuration(uint maxDuration)
        {
            return _dbcontext.Events
                   .Where(bgEvent => !bgEvent.Deleted && bgEvent.Duration <= maxDuration)
                   .ToList();
        }

        public List<BoardGameEvent> GetByCost(uint maxCost)
        {
            return _dbcontext.Events
                   .Where(bgEvent => !bgEvent.Deleted && bgEvent.Cost <= maxCost)
                   .ToList();
        }

        public List<BoardGameEvent> GetByPurchase(bool purchase)
        {
            return _dbcontext.Events
                   .Where(bgEvent => !bgEvent.Deleted && bgEvent.Purchase == purchase)
                   .ToList();
        }

        public List<BoardGameEvent> GetByState(BoardGameEventState state)
        {
            return _dbcontext.Events
                   .Where(bgEvent => !bgEvent.Deleted && bgEvent.State == state)
                   .ToList();
        }

        public List<BoardGame> GetEventGames(long bgEventID)
        {
            return _dbcontext.Games
                    .Join(_dbcontext.EventGameRelations,
                          g => g.ID,
                          egr => egr.BoardGameID,
                          (g, egr) => new { g, egr.BoardGameEventID })
                    .Where(r => r.BoardGameEventID == bgEventID)
                    .Select(r => r.g)
                    .ToList();
        }
        public List<Player> GetEventPlayers(long bgEventID)
        {
            return _dbcontext.Players
                    .Join(_dbcontext.Registrations,
                          p => p.ID,
                          r => r.PlayerID,
                          (p, r) => new { p, r.BoardGameEventID })
                    .Where(r => r.BoardGameEventID == bgEventID)
                    .Select(r => r.p)
                    .ToList();
        }
    }
}
