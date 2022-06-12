using BusinessLogic.Models;
using BusinessLogic.IRepositories;
using BusinessLogic.Exceptions;

namespace BusinessLogic.Services
{
    public interface IPlayerService
    {
        List<Player> GetPlayers();
        void CreatePlayer(Player player);
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

        public void CreatePlayer(Player player)
        {
            if (Exist(player))
                throw new AlreadyExistsPlayerException();

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

        private bool Exist(Player player)
        {
             return _playerRepository.GetByName(player.Name) != null;
        }

        private bool NotExist(long id)
        {
            return _playerRepository.GetByID(id) == null;
        }
    }
}
