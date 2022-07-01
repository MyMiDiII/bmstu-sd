using Microsoft.EntityFrameworkCore;

using BusinessLogic.Models;

namespace DataAccess
{
    public class AppContext : DbContext
    {
        public DbSet<Venue> Venues { get; set; }
        public DbSet<Organizer> Organizers { get; set; }
        public DbSet<BoardGameEvent> Events { get; set; }

        public AppContext(DbContextOptions<AppContext> options) : base(options) { }
    }
}
