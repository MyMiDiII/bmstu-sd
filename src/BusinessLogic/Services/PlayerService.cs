﻿using BusinessLogic.Models;
using BusinessLogic.IRepositories;
using BusinessLogic.Exceptions;

namespace BusinessLogic.Services
{
    public interface IPlayerService
    {
        List<Player> GetPlayers();
        void CreatePlayer(string name);
        void UpdatePlayer(Player player);
        void DeletePlayer(Player player);
        Player? GetPlayerByID(long id);
        Player? GetPlayerByName(string name);
        void RegisterCurrentPlayerForEvent(BoardGameEvent bgEvent);
        void UnregisterCurrentPlayerForEvent(BoardGameEvent bgEvent);
        List<BoardGameEvent> GetCurrentPlayerEvents();
        void AddBoardGameToFavorite(BoardGame boardGame);
        void DeleteBoardGameFromFavorite(BoardGame boardGame);
        List<BoardGame> GetCurrentPlayerFavorites();
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

            var player = new Player(name);

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

        private long GetCurrentPlayerID()
        {
            long playerID = _userService.GetCurrentUserRoleID("player");

            if (playerID == -1)
                throw new UserIsNotPlayerException();

            return playerID;
        }

        public void RegisterCurrentPlayerForEvent(BoardGameEvent bgEvent)
        {
            long playerID = GetCurrentPlayerID();

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
            long playerID = GetCurrentPlayerID();

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
            long playerID = GetCurrentPlayerID();
            return _playerRepository.GetPlayerEvents(playerID);
        }

        public void AddBoardGameToFavorite(BoardGame boardGame)
        {
            long playerID = GetCurrentPlayerID();

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
            long playerID = GetCurrentPlayerID();

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
            long playerID = GetCurrentPlayerID();
            return _playerRepository.GetPlayerFavorites(playerID);
        }
    }
}