using BusinessLogic.Models;
using BusinessLogic.IRepositories;
using BusinessLogic.Exceptions;

namespace BusinessLogic.Services
{
    public interface IVenueService
    {
        List<Venue> GetVenues();
        public Venue? GetVenueByID(long id);
        void CreateVenue(Venue venue);
        void UpdateVenue(Venue venue);
        void DeleteVenue(Venue venue);
    }

    public class VenueService : IVenueService
    {
        private readonly IVenueRepository _venueRepository;

        public VenueService(IVenueRepository venueRepository)
        {
            _venueRepository = venueRepository;
        }

        public List<Venue> GetVenues()
        {
            return _venueRepository.GetAll();
        }

        public Venue? GetVenueByID(long id)
        {
            return _venueRepository.GetByID(id);
        }

        public void CreateVenue(Venue venue)
        {
            if (Exist(venue))
                throw new AlreadyExistsVenueException();

            _venueRepository.Add(venue);
        }

        public void UpdateVenue(Venue venue)
        {
            if (NotExist(venue.ID))
                throw new NotExistsVenueException();

            _venueRepository.Update(venue);
        }

        public void DeleteVenue(Venue venue)
        {
            if (NotExist(venue.ID))
                throw new NotExistsVenueException();

            _venueRepository.Delete(venue);
        }

        private bool Exist(Venue venue)
        {
             return _venueRepository.GetAll().Any(elem
                        => elem.Name == venue.Name
                        && elem.Type == venue.Type
                        && elem.Address == venue.Address);
        }

        private bool NotExist(long id)
        {
            return _venueRepository.GetByID(id) == null;
        }
    }
}
