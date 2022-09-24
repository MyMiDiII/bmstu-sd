using BusinessLogic.Models;
using BusinessLogic.IRepositories;
using BusinessLogic.Exceptions;

namespace BusinessLogic.Services
{
    public interface IProducerService
    {
        List<Producer> GetProducers();
        Producer? GetProducerByID(long id);
        void CreateProducer(Producer venue);
        void UpdateProducer(Producer venue);
        void DeleteProducer(Producer venue);
    }

    public class ProducerService : IProducerService
    {
        private readonly IProducerRepository _venueRepository;

        public ProducerService(IProducerRepository venueRepository)
        {
            _venueRepository = venueRepository;
        }

        public List<Producer> GetProducers()
        {
            return _venueRepository.GetAll();
        }

        public Producer? GetProducerByID(long id)
        {
            return _venueRepository.GetByID(id);
        }

        public void CreateProducer(Producer venue)
        {
            if (Exist(venue))
                throw new AlreadyExistsProducerException();

            _venueRepository.Add(venue);
        }

        public void UpdateProducer(Producer venue)
        {
            if (NotExist(venue.ID))
                throw new NotExistsProducerException();

            _venueRepository.Update(venue);
        }

        public void DeleteProducer(Producer venue)
        {
            if (NotExist(venue.ID))
                throw new NotExistsProducerException();

            _venueRepository.Delete(venue);
        }

        private bool Exist(Producer venue)
        {
            return _venueRepository.GetAll().Any(elem
                       => elem.Name == venue.Name);
        }

        private bool NotExist(long id)
        {
            return _venueRepository.GetByID(id) == null;
        }
    }
}
