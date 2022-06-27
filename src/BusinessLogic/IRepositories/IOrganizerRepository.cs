using BusinessLogic.Models;

namespace BusinessLogic.IRepositories
{
    public interface IOrganizerRepository : IRepository<Organizer>
    {
        List<Organizer> GetByName(string name);
        List<Organizer> GetByAddress(string address);
        List<BoardGameEvent> GetOrganizerEvents(long organizerID);
    }
}
