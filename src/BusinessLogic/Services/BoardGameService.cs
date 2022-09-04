using BusinessLogic.Models;
using BusinessLogic.IRepositories;
using BusinessLogic.Exceptions;

namespace BusinessLogic.Services
{
    public interface IBoardGameService
    {
        BoardGame? GetBoardGameByID(long id);
        List<BoardGame> GetBoardGames();
        void CreateBoardGame(BoardGame boardGame);
        void UpdateBoardGame(BoardGame boardGame);
        void DeleteBoardGame(BoardGame boardGame);
        void AddBoardGameToFavorite(BoardGame boardGame);
        void DeleteBoardGameFromFavorite(BoardGame boardGame);
        bool CheckBoardGameInCurrentUserFavorites(BoardGame boardGame);
        List<BoardGameEvent> GetEventsByGame(BoardGame boardGame);
        void AddBoardGameToEvent(BoardGame boardGame, BoardGameEvent bgEvent);
        void DeleteBoardGameFromEvent(BoardGame boardGame, BoardGameEvent bgEvent);
    }

    public class BoardGameService : IBoardGameService
    {
        private readonly IBoardGameRepository _boardGameRepository;
        private readonly IPlayerService _playerService;

        public BoardGameService(IBoardGameRepository boardGameRepository, IPlayerService playerService)
        {
            _boardGameRepository = boardGameRepository;
            _playerService = playerService;
        }

        public BoardGame? GetBoardGameByID(long id)
        {
            return _boardGameRepository.GetByID(id);
        }

        public List<BoardGame> GetBoardGames()
        {
            return _boardGameRepository.GetAll();
        }

        public void CreateBoardGame(BoardGame boardGame)
        {
            if (Exist(boardGame))
                throw new AlreadyExistsBoardGameException();

            _boardGameRepository.Add(boardGame);
        }

        public void UpdateBoardGame(BoardGame boardGame)
        {
            if (NotExist(boardGame.ID))
                throw new NotExistsBoardGameException();

            _boardGameRepository.Update(boardGame);
        }

        public void DeleteBoardGame(BoardGame boardGame)
        {
            if (NotExist(boardGame.ID))
                throw new NotExistsBoardGameException();

            _boardGameRepository.Delete(boardGame);
        }

        private bool Exist(BoardGame boardGame)
        {
             return _boardGameRepository.GetAll().Any(elem
                        => elem.Title == boardGame.Title
                        && elem.Producer == boardGame.Producer
                        && elem.Year == boardGame.Year);
        }

        private bool NotExist(long id)
        {
            return _boardGameRepository.GetByID(id) == null;
        }

        public void AddBoardGameToFavorite(BoardGame boardGame)
        {
            long playerID = _playerService.GetCurrentPlayerID();

            if (_boardGameRepository.CheckGameInFavorites(boardGame.ID, playerID))
                throw new AlreadyExistsFavoriteGameException();

            _boardGameRepository.AddToFavorites(boardGame.ID, playerID);
        }

        public void DeleteBoardGameFromFavorite(BoardGame boardGame)
        {
            long playerID = _playerService.GetCurrentPlayerID();

            if (!_boardGameRepository.CheckGameInFavorites(boardGame.ID, playerID))
                throw new NotExistsFavoriteGameException();

            _boardGameRepository.DeleteFromFavorites(boardGame.ID, playerID);
        }

        public bool CheckBoardGameInCurrentUserFavorites(BoardGame boardGame)
        {
            long playerID = _playerService.GetCurrentPlayerID();
            return _boardGameRepository.CheckGameInFavorites(boardGame.ID, playerID);
        }

        public List<BoardGameEvent> GetEventsByGame(BoardGame boardGame)
        {
            return _boardGameRepository.GetGameEvents(boardGame.ID);
        }

        public void AddBoardGameToEvent(BoardGame boardGame, BoardGameEvent bgEvent)
        {
            if (_boardGameRepository.CheckGamePlaying(boardGame.ID, bgEvent.ID))
                throw new AlreadyExistsEventGameException();

            _boardGameRepository.AddToEvent(boardGame.ID, bgEvent.ID);
        }

        public void DeleteBoardGameFromEvent(BoardGame boardGame, BoardGameEvent bgEvent)
        {
            if (!_boardGameRepository.CheckGamePlaying(boardGame.ID, bgEvent.ID))
                throw new NotExistsEventGameException();

            _boardGameRepository.DeleteFromEvent(boardGame.ID, bgEvent.ID);
        }
    }
}
