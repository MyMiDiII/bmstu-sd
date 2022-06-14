using BusinessLogic.Models;
using BusinessLogic.IRepositories;
using BusinessLogic.Exceptions;
using BusinessLogic.Config;

namespace BusinessLogic.Services
{
    public interface IPlayerService
    {
        List<Player> GetPlayers();
        void CreatePlayer(string name);
        void UpdatePlayer(Player player);
        void DeletePlayer(Player player);
        public Player? GetPlayerByID(long id);
        public Player? GetPlayerByName(string name);
        public void RegisterCurrentPlayerForEvent(BoardGameEvent bgEvent);
        public void UnregisterCurrentPlayerForEvent(BoardGameEvent bgEvent);
        public List<BoardGameEvent> GetCurrentPlayerEvents();
        public void AddBoardGameToFavorite(BoardGame boardGame);
        public void DeleteBoardGameFromFavorite(BoardGame boardGame);
        public List<BoardGame> GetCurrentPlayerFavorites();
    }

    public class PlayerService : IPlayerService
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly IUserService _userService;

        public PlayerService(IPlayerRepository playerRepository, IUserService userService)
        {
            _playerRepository = playerRepository;
            _userService = userService;
        }

        public List<Player> GetPlayers()
        {
            return _playerRepository.GetAll();
        }

        public void CreatePlayer(string name)
        {
            if (Exist(name))
                throw new AlreadyExistsPlayerException();

            var player = new Player
            {
                Name = name,
                League = PlayerConfig.Leagues.First(),
                Rating = 0
            };

            _playerRepository.Add(player);
        }

        public void UpdatePlayer(Player player)
        {
            if (NotExist(player.ID))
                throw new NotExistsPlayerException();

            _playerRepository.Update(player);
        }

        public void DeletePlayer(Player player)
        {
            if (NotExist(player.ID))
                throw new NotExistsPlayerException();

            _playerRepository.Delete(player);
        }

        private bool Exist(string name)
        {
            return _playerRepository.GetByName(name) != null;
        }

        private bool NotExist(long id)
        {
            return _playerRepository.GetByID(id) == null;
        }

        public Player? GetPlayerByID(long id)
        {
            return _playerRepository.GetByID(id);
        }

        public Player? GetPlayerByName(string name)
        {
            return _playerRepository.GetByName(name);
        }

        public void RegisterCurrentPlayerForEvent(BoardGameEvent bgEvent)
        {
            User curUser = _userService.GetCurrentUser();

            if (curUser.Role != "player")
                throw new UserIsNotPlayerException();

            long playerID = curUser.RoleID;

            var curRegistation = new BGERegistration()
            {
                BoardGameEventID = bgEvent.ID,
                PlayerID = playerID,
            };

            if (_playerRepository.GetRegistrationID(curRegistation) != -1)
                throw new AlreadyExistsPlayerRegistraionException();

            _playerRepository.AddToEvent(curRegistation);
        }

        public void UnregisterCurrentPlayerForEvent(BoardGameEvent bgEvent)
        {
            User curUser = _userService.GetCurrentUser();

            if (curUser.Role != "player")
                throw new UserIsNotPlayerException();

            long playerID = curUser.RoleID;

            var curRegistation = new BGERegistration()
            {
                BoardGameEventID = bgEvent.ID,
                PlayerID = playerID,
            };

            if (_playerRepository.GetRegistrationID(curRegistation) == -1)
                throw new NotExistsPlayerRegistraionException();

            _playerRepository.DeleteFromEvent(curRegistation);
        }

        public List<BoardGameEvent> GetCurrentPlayerEvents()
        {
            User curUser = _userService.GetCurrentUser();

            if (curUser.Role != "player")
                throw new UserIsNotPlayerException();

            long playerID = curUser.RoleID;

            return _playerRepository.GetPlayerEvents(playerID);
        }

        public void AddBoardGameToFavorite(BoardGame boardGame)
        {
            User curUser = _userService.GetCurrentUser();

            if (curUser.Role != "player")
                throw new UserIsNotPlayerException();

            long playerID = curUser.RoleID;

            var curFavorite = new FavoriteBoardGame()
            {
                BoardGameID = boardGame.ID,
                PlayerID = playerID
            };

            if (_playerRepository.GetFavoriteID(curFavorite) != -1)
                throw new AlreadyExistsPlayerFavoriteGameException();

            _playerRepository.AddToPlayer(curFavorite);
        }

        public void DeleteBoardGameFromFavorite(BoardGame boardGame)
        {
            User curUser = _userService.GetCurrentUser();

            if (curUser.Role != "player")
                throw new UserIsNotPlayerException();

            long playerID = curUser.RoleID;

            var curFavorite = new FavoriteBoardGame()
            {
                BoardGameID = boardGame.ID,
                PlayerID = playerID
            };

            if (_playerRepository.GetFavoriteID(curFavorite) == -1)
                throw new NotExistsPlayerFavoriteGameException();

            _playerRepository.DeleteFromPlayer(curFavorite);
        }

        public List<BoardGame> GetCurrentPlayerFavorites()
        {
            User curUser = _userService.GetCurrentUser();

            if (curUser.Role != "player")
                throw new UserIsNotPlayerException();

            long playerID = curUser.RoleID;

            return _playerRepository.GetPlayerFavorites(playerID);
        }
    }
}
