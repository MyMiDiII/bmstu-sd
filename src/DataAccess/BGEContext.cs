using Microsoft.EntityFrameworkCore;

using BusinessLogic.Models;

namespace DataAccess
{
    public class BGEContext : DbContext
    {
        public DbSet<Venue> Venues { get; set; }
        public DbSet<Organizer> Organizers { get; set; }
        public DbSet<BoardGameEvent> Events { get; set; }

        public BGEContext(DbContextOptions<BGEContext> options) : base(options) { }
    }
}
