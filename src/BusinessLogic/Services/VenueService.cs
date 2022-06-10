using BusinessLogic.Models;
using BusinessLogic.IRepositories;

namespace BusinessLogic.Services
{
    public class VenueService : IVenueService
    {
        private IVenueRepository _venueRepository;

        public VenueService(IVenueRepository venueRepository)
        {
            _venueRepository = venueRepository;
        }

        public List<Venue> GetVenues()
        {
            return _venueRepository.GetAll();
        }
    }

    public interface IVenueService
    {
        List<Venue> GetVenues();
    }
}
