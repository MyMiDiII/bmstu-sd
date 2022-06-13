using BusinessLogic.Models;
using BusinessLogic.IRepositories;
using BusinessLogic.Exceptions;
using BusinessLogic.Config;

namespace BusinessLogic.Services
{
    public interface IPlayerService
    {
        List<Player> GetPlayers();
        public Player? GetPlayerByID(long id);
        public Player? GetPlayerByName(string name);
        public List<Player> GetPlayersByEvent(long eventID);
        void CreatePlayer(string name);
        void UpdatePlayer(Player player);
        void DeletePlayer(Player player);
    }

    public class PlayerService : IPlayerService
    {
        private IPlayerRepository _playerRepository;

        public PlayerService(IPlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
        }

        public List<Player> GetPlayers()
        {
            return _playerRepository.GetAll();
        }

        public Player? GetPlayerByID(long id)
        {
            return _playerRepository.GetByID(id);
        }

        public Player? GetPlayerByName(string name)
        {
            return _playerRepository.GetByName(name);
        }

        public List<Player> GetPlayersByEvent(long eventID)
        {
            return _playerRepository.GetByEvent(eventID);
        }

        public void CreatePlayer(string name)
        {
            if (Exist(name))
                throw new AlreadyExistsPlayerException();

            Player player = new Player
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
    }
}
