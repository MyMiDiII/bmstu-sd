using BusinessLogic.Models;
using BusinessLogic.IRepositories;
using BusinessLogic.Exceptions;

namespace BusinessLogic.Services
{
    public interface IBoardGameEventService
    {
        BoardGameEvent? GetBoardGameEventByID(long id);
        List<BoardGameEvent> GetBoardGameEvents();
        void CreateBoardGameEvent(BoardGameEvent boardGameEvent);
        void UpdateBoardGameEvent(BoardGameEvent boardGameEvent);
        void DeleteBoardGameEvent(BoardGameEvent boardGameEvent);
        List<BoardGame> GetGamesByEvent(BoardGameEvent boardGameEvent);
        List<Player> GetPlayersByEvent(BoardGameEvent boardGameEvent);
    }

    public class BoardGameEventService : IBoardGameEventService
    {
        private readonly IBoardGameEventRepository _boardGameEventRepository;

        public BoardGameEventService(IBoardGameEventRepository boardGameEventRepository)
        {
            _boardGameEventRepository = boardGameEventRepository;
        }

        public BoardGameEvent? GetBoardGameEventByID(long id)
        {
            return _boardGameEventRepository.GetByID(id);
        }

        public List<BoardGameEvent> GetBoardGameEvents()
        {
            return _boardGameEventRepository.GetAll();
        }

        public void CreateBoardGameEvent(BoardGameEvent boardGameEvent)
        {
            if (Exist(boardGameEvent))
                throw new AlreadyExistsBoardGameEventException();

            _boardGameEventRepository.Add(boardGameEvent);
        }

        public void UpdateBoardGameEvent(BoardGameEvent boardGameEvent)
        {
            if (NotExist(boardGameEvent.ID))
                throw new NotExistsBoardGameEventException();

            _boardGameEventRepository.Update(boardGameEvent);
        }

        public void DeleteBoardGameEvent(BoardGameEvent boardGameEvent)
        {
            if (NotExist(boardGameEvent.ID))
                throw new NotExistsBoardGameEventException();

            _boardGameEventRepository.Delete(boardGameEvent);
        }

        private bool Exist(BoardGameEvent boardGameEvent)
        {
             return _boardGameEventRepository.GetAll().Any(elem
                        => elem.Title == boardGameEvent.Title
                        && elem.Date == boardGameEvent.Date
                        && elem.OrganizerID == boardGameEvent.OrganizerID
                        && elem.VenueID == boardGameEvent.VenueID);
        }

        private bool NotExist(long id)
        {
            return _boardGameEventRepository.GetByID(id) == null;
        }

        public List<BoardGame> GetGamesByEvent(BoardGameEvent bgEvent)
        {
            return _boardGameEventRepository.GetEventGames(bgEvent.ID);
        }

        public List<Player> GetPlayersByEvent(BoardGameEvent bgEvent)
        {
            return _boardGameEventRepository.GetEventPlayers(bgEvent.ID);
        }

    }
}
