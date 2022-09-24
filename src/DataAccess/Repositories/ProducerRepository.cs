using BusinessLogic.Models;
using BusinessLogic.IRepositories;
using BusinessLogic.Exceptions;

namespace DataAccess.Repositories
{
    public class ProducerRepository : IProducerRepository
    {
        private readonly BGEContext _dbcontext;

        public ProducerRepository(BGEContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public long Add(Producer elem)
        {
            try
            {
                _dbcontext.Producers.Add(elem);
                _dbcontext.SaveChanges();

                return elem.ID;
            }
            catch
            {
                throw new AddProducerException();
            }
        }

        public List<Producer> GetAll()
        {
            return _dbcontext.Producers.Where(producer => !producer.Deleted).ToList();
        }

        public Producer? GetByID(long id)
        {
            return _dbcontext.Producers.Find(id);
        }

        public void Update(Producer elem)
        {
            try
            {
                _dbcontext.Producers.Update(elem);
                _dbcontext.SaveChanges();
            }
            catch
            {
                throw new UpdateProducerException();
            }
       }

        public void Delete(Producer elem)
        {
            var tmp = _dbcontext.Producers.Find(elem.ID);

            if (tmp is null)
                throw new NotExistsProducerException();

            if (tmp.Deleted)
                throw new AlreadyDeletedProducerException();

            tmp.Deleted = true;
            Update(tmp);
        }

        public List<Producer> GetByName(string name)
        {
            return _dbcontext.Producers
                   .Where(producer => !producer.Deleted && producer.Name.Contains(name))
                   .ToList();
        }

        public List<Producer> GetByRating(long rating)
        {
            return _dbcontext.Producers
                   .Where(producer => !producer.Deleted && producer.Rating == rating)
                   .ToList();
        }

    }
}
