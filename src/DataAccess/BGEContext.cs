using Microsoft.EntityFrameworkCore;

using BusinessLogic.Models;

namespace DataAccess
{
    public class BGEContext : DbContext
    {
        public DbSet<Venue> Venues { get; set; }
        public DbSet<Organizer> Organizers { get; set; }
        public DbSet<BoardGameEvent> Events { get; set; }
        public DbSet<BoardGame> Games { get; set; }
        public DbSet<Player> Players { get; set; }

        public DbSet<EventGame> EventGameRelations { get; set; }
        public DbSet<PlayerRegistration> Registrations { get; set; }
        public DbSet<FavoriteBoardGame> Favorites { get; set; }

        public BGEContext(DbContextOptions<BGEContext> options) : base(options) { }
    }
}
