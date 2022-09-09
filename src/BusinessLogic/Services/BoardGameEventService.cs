using BusinessLogic.Models;
using BusinessLogic.IRepositories;
using BusinessLogic.Exceptions;

namespace BusinessLogic.Services
{
    public interface IBoardGameEventService
    {
        BoardGameEvent? GetBoardGameEventByID(long id);
        List<BoardGameEvent> GetBoardGameEvents();
        long CreateBoardGameEvent(BoardGameEvent boardGameEvent);
        void UpdateBoardGameEvent(BoardGameEvent boardGameEvent);
        void DeleteBoardGameEvent(BoardGameEvent boardGameEvent);
        List<BoardGame> GetGamesByEvent(BoardGameEvent boardGameEvent);
        List<Player> GetPlayersByEvent(BoardGameEvent boardGameEvent);
        Organizer? GetOrganizerByEvent(BoardGameEvent boardGameEvent);
        Venue? GetVenueByEvent(BoardGameEvent boardGameEvent);
    }

    public class BoardGameEventService : IBoardGameEventService
    {
        private readonly IBoardGameEventRepository _boardGameEventRepository;
        private readonly IOrganizerRepository _organizerRepository;
        private readonly IVenueRepository _venueRepository;

        public BoardGameEventService(IBoardGameEventRepository boardGameEventRepository,
                                     IOrganizerRepository organizerRepository,
                                     IVenueRepository venueRepository)
        {
            _boardGameEventRepository = boardGameEventRepository;
            _organizerRepository = organizerRepository;
            _venueRepository = venueRepository;
        }

        public BoardGameEvent? GetBoardGameEventByID(long id)
        {
            return _boardGameEventRepository.GetByID(id);
        }

        public List<BoardGameEvent> GetBoardGameEvents()
        {
            return _boardGameEventRepository.GetAll();
        }

        public long CreateBoardGameEvent(BoardGameEvent boardGameEvent)
        {
            if (Exist(boardGameEvent))
                throw new AlreadyExistsBoardGameEventException();

            return _boardGameEventRepository.Add(boardGameEvent);
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

        public Organizer? GetOrganizerByEvent(BoardGameEvent boardGameEvent)
        {
            return _organizerRepository.GetByID(boardGameEvent.OrganizerID);
        }

        public Venue? GetVenueByEvent(BoardGameEvent boardGameEvent)
        {
            return _venueRepository.GetByID(boardGameEvent.VenueID);
        }
    }
}
