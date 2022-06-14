using BusinessLogic.Models;
using BusinessLogic.IRepositories;
using BusinessLogic.Exceptions;

namespace BusinessLogic.Services
{
    public interface IBoardGameService
    {
        List<BoardGame> GetBoardGames();
        void CreateBoardGame(BoardGame boardGame);
        void UpdateBoardGame(BoardGame boardGame);
        void DeleteBoardGame(BoardGame boardGame);
    }

    public class BoardGameService : IBoardGameService
    {
        private readonly IBoardGameRepository _boardGameRepository;

        public BoardGameService(IBoardGameRepository boardGameRepository)
        {
            _boardGameRepository = boardGameRepository;
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
                        && elem.Produser == boardGame.Produser
                        && elem.Year == boardGame.Year);
        }

        private bool NotExist(long id)
        {
            return _boardGameRepository.GetByID(id) == null;
        }
    }
}
