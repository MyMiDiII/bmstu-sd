using BusinessLogic.Models;
using BusinessLogic.IRepositories;
using BusinessLogic.Exceptions;

namespace DataAccess.Repositories
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly BGEContext _dbcontext;

        public PlayerRepository(BGEContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public void Add(Player elem)
        {
            try
            {
                _dbcontext.Players.Add(elem);
                _dbcontext.SaveChanges();
            }
            catch
            {
                throw new AddPlayerException();
            }
        }

        public List<Player> GetAll()
        {
            return _dbcontext.Players.Where(player => !player.Deleted).ToList();
        }

        public Player? GetByID(long id)
        {
            return _dbcontext.Players.Find(id);
        }

        public void Update(Player elem)
        {
            try
            {
                _dbcontext.Players.Update(elem);
                _dbcontext.SaveChanges();
            }
            catch
            {
                throw new UpdatePlayerException();
            }
        }

        public void Delete(Player elem)
        {
            var tmp = _dbcontext.Players.Find(elem.ID);

            if (tmp is null)
                throw new NotExistsPlayerException();

            if (tmp.Deleted)
                throw new AlreadyDeletedPlayerException();

            tmp.Deleted = true;
            Update(tmp);
        }

        public Player? GetByName(string name)
        {
            return _dbcontext.Players
                   .SingleOrDefault(player => player.Name.Contains(name));
        }

        public List<Player> GetByLeague(string league)
        {
            return _dbcontext.Players
                   .Where(player => !player.Deleted && player.League == league)
                   .ToList();
        }

        public List<Player> GetByRating(uint rating)
        {
            return _dbcontext.Players
                   .Where(player => !player.Deleted && player.Rating == rating)
                   .ToList();
        }

        public void AddToEvent(long eventID, long playerID)
        {
            if (CheckPlayerRegistration(playerID, eventID))
                throw new AlreadyExistsPlayerRegistraionException();

            var newPlayerReg = new PlayerRegistration(playerID, eventID);

            try
            {
                _dbcontext.Registrations.Add(newPlayerReg);
                _dbcontext.SaveChanges();
            }
            catch
            {
                throw new AddEventGameException();
            }
        }

        public void DeleteFromEvent(long eventID, long playerID)
        {
            try
            {
                var registration = _dbcontext.Registrations
                                   .First(eg => eg.PlayerID == playerID && eg.BoardGameEventID == eventID);

                _dbcontext.Registrations.Remove(registration);
                _dbcontext.SaveChanges();
            }
            catch
            {
                throw new DeleteEventGameException();
            }
        }

        public bool CheckPlayerRegistration(long eventID, long playerID)
        {
            return _dbcontext.Registrations
                   .Where(reg
                          => reg.PlayerID == playerID
                          && reg.BoardGameEventID == eventID)
                   .Any();
        }

        public List<BoardGameEvent> GetPlayerEvents(long playerID)
        {
            return _dbcontext.Events
                    .Join(_dbcontext.Registrations,
                          e => e.ID,
                          reg => reg.BoardGameEventID,
                          (e, reg) => new { e, reg.PlayerID })
                    .Where(r => r.PlayerID == playerID)
                    .Select(r => r.e)
                    .ToList();
        }

        public List<BoardGame> GetPlayerFavorites(long playerID)
        {
            return _dbcontext.Games
                    .Join(_dbcontext.Favorites,
                          e => e.ID,
                          reg => reg.BoardGameID,
                          (e, reg) => new { e, reg.PlayerID })
                    .Where(r => r.PlayerID == playerID)
                    .Select(r => r.e)
                    .ToList();
        }
    }
}
