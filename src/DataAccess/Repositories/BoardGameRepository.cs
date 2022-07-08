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

        public void AddToEvent(long gameID, long eventID)
        {
            if (CheckGamePlaying(gameID, eventID))
                throw new AlreadyExistsEventGameException();

            var newEventGame = new EventGame(gameID, eventID);

            try
            {
                _dbcontext.EventGameRelations.Add(newEventGame);
                _dbcontext.SaveChanges();
            }
            catch
            {
                throw new AddEventGameException();
            }
        }

        public void DeleteFromEvent(long gameID, long eventID)
        {
            try
            {
                var eventGame = _dbcontext.EventGameRelations
                                .First(eg => eg.BoardGameID == gameID && eg.BoardGameEventID == eventID);

                _dbcontext.EventGameRelations.Remove(eventGame);
                _dbcontext.SaveChanges();
            }
            catch
            {
                throw new DeleteEventGameException();
            }
        }

        public bool CheckGamePlaying(long gameID, long eventID)
        {
            return _dbcontext.EventGameRelations
                   .Where(egr
                          => egr.BoardGameID == gameID
                          && egr.BoardGameEventID == eventID)
                   .Any();
        }

        public List<BoardGameEvent> GetGameEvents(long gameID)
        {
            return _dbcontext.Events
                    .Join(_dbcontext.EventGameRelations,
                          e => e.ID,
                          egr => egr.BoardGameEventID,
                          (e, egr) => new { e, egr.BoardGameID })
                    .Where(r => r.BoardGameID == gameID)
                    .Select(r => r.e)
                    .ToList();
        }

        public void AddToFavorites(long gameID, long playerID)
        {
            if (CheckGameInFavorites(gameID, playerID))
                throw new AlreadyExistsFavoriteGameException();

            var newFavorite = new FavoriteBoardGame(gameID, playerID);

            try
            {
                _dbcontext.Favorites.Add(newFavorite);
                _dbcontext.SaveChanges();
            }
            catch
            {
                throw new AddFavoriteGameException();
            }
        }

        public void DeleteFromFavorites(long gameID, long playerID)
        {
            try
            {
                var playerGame = _dbcontext.Favorites
                                .First(eg => eg.BoardGameID == gameID && eg.PlayerID == playerID);

                _dbcontext.Favorites.Remove(playerGame);
                _dbcontext.SaveChanges();
            }
            catch
            {
                throw new DeleteFavoriteGameException();
            }
        }

        public bool CheckGameInFavorites(long gameID, long playerID)
        {
            return _dbcontext.Favorites
                   .Where(egr
                          => egr.BoardGameID == gameID
                          && egr.PlayerID == playerID)
                   .Any();
        }
    }
}
