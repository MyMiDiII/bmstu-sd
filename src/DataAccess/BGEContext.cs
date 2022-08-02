using Microsoft.EntityFrameworkCore;

using BusinessLogic.Models;

namespace DataAccess
{
    public class BGEContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        public DbSet<Venue> Venues { get; set; }
        public DbSet<Organizer> Organizers { get; set; }
        public DbSet<BoardGameEvent> Events { get; set; }
        public DbSet<BoardGame> Games { get; set; }
        public DbSet<Player> Players { get; set; }

        public DbSet<EventGame> EventGameRelations { get; set; }
        public DbSet<PlayerRegistration> Registrations { get; set; }
        public DbSet<FavoriteBoardGame> Favorites { get; set; }

        public BGEContext(DbContextOptions<BGEContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<EventGame>().HasKey(eg => new { eg.BoardGameID, eg.BoardGameEventID });
            builder.Entity<PlayerRegistration>().HasKey(pr => new { pr.BoardGameEventID, pr.PlayerID });
            builder.Entity<FavoriteBoardGame>().HasKey(fbg => new { fbg.BoardGameID, fbg.PlayerID });

            builder.Entity<User>().HasData( new User("guest", "guest") { ID = 1 } );
            builder.Entity<Role>().HasData( new Role("guest") { ID = 1, UserID = 1 } );
        }
    }
}
