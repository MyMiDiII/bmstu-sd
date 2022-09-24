using System.Data.Common;

using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;

using DataAccess;
using DataAccess.Repositories;
using BusinessLogic.Models;

namespace DataAccessTests
{
    public class OrganizerRepositoryTests
    {
        private readonly DbConnection _dbconnection;
        private readonly DbContextOptions<BGEContext> _dbContextOptions;

        public OrganizerRepositoryTests()
        {
            _dbconnection = new SqliteConnection("Filename=:memory:");
            _dbconnection.Open();

            _dbContextOptions = new DbContextOptionsBuilder<BGEContext>()
                .UseSqlite(_dbconnection)
                .Options;

            using var context = new BGEContext(_dbContextOptions);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.Organizers.AddRange(new Organizer("O1", "Москва"));
            context.Venues.AddRange(new Venue("V1", "кафе", "Москва"));
            context.SaveChanges();

            context.Events.AddRange(new BoardGameEvent("Yay!", new DateOnly(2022, 7, 5))
                                                       { OrganizerID = 1, VenueID = 1 });
            context.SaveChanges();
        }

        private BGEContext CreateContext() => new(_dbContextOptions);
        private OrganizerRepository CreateOrganizerRepository() => new(CreateContext());
        private static OrganizerRepository CreateOrganizerRepository(BGEContext context) =>
            new(context);

        [Fact]
        public void OrganizerGetAllTest()
        {
            var rep = CreateOrganizerRepository();
            var organizers = rep.GetAll();

            Assert.Single(organizers);
            Assert.Collection(
                organizers,
                v =>
                {
                    Assert.Equal(1, v.ID);
                    Assert.Equal("O1", v.Name);
                    Assert.Equal("Москва", v.Address);
                }
            );
        }

        [Fact]
        public void OrganizerGetByIDTest()
        {
            var rep = CreateOrganizerRepository();
            var organizer = rep.GetByID(1);

            Assert.NotNull(organizer);
            Assert.Equal(1, organizer?.ID);
            Assert.Equal("O1", organizer?.Name);
            Assert.Equal("Москва", organizer?.Address);
        }

        [Fact]
        public void OrganizerAddTest()
        {
            var context = CreateContext();
            var rep = CreateOrganizerRepository(context);
            var organizer = new Organizer("O2", "Орел");

            rep.Add(organizer);

            Assert.Equal(2, context.Organizers.Count());
            var added = context.Organizers.Single(v => v.Name == "O2");
            Assert.NotNull(added);
            Assert.Equal(2, added?.ID);
            Assert.Equal("O2", added?.Name);
            Assert.Equal("Орел", added?.Address);
        }

        [Fact]
        public void OrganizerUpdateTest()
        {
            var context = CreateContext();
            var rep = CreateOrganizerRepository(context);
            var organizer = new Organizer("ThEBeST", "Москва") { ID = 1 };

            rep.Update(organizer);

            Assert.Single(context.Organizers);
            Assert.Null(context.Organizers.FirstOrDefault(v => v.Name == "O1"));
            var updated = context.Organizers.Single(v => v.Name == "ThEBeST");
            Assert.NotNull(updated);
            Assert.Equal(1, updated?.ID);
            Assert.Equal("ThEBeST", updated?.Name);
            Assert.Equal("Москва", updated?.Address);
        }

        [Fact]
        public void OrganizerDeleteTest()
        {
            var context = CreateContext();
            var rep = CreateOrganizerRepository(context);
            var organizer = new Organizer("O1", "Москва") { ID = 1 };

            rep.Delete(organizer);

            Assert.Single(context.Organizers);
            var deleted = context.Organizers.Single(v => v.Name == "O1");
            Assert.NotNull(deleted);
            Assert.True(deleted.Deleted);
            Assert.Equal(1, deleted?.ID);
            Assert.Equal("O1", deleted?.Name);
            Assert.Equal("Москва", deleted?.Address);
        }

        [Fact]
        public void OrganizerGetByAddressTest()
        {
            var rep = CreateOrganizerRepository();

            var venues = rep.GetByAddress("Москва");

            Assert.NotNull(venues);
            Assert.NotEmpty(venues);
            var found = venues.FirstOrDefault(v => v.Name == "O1");
            Assert.NotNull(found);
            Assert.Equal(1, found?.ID);
            Assert.Equal("O1", found?.Name);
            Assert.Equal("Москва", found?.Address);
        }

        [Fact]
        public void OrganizerGetEventsTest()
        {
            var context = CreateContext();
            var rep = CreateOrganizerRepository(context);

            var events = rep.GetOrganizerEvents(1);

            Assert.Single(events);
            Assert.Collection(
                events,
                e =>
                {
                    Assert.Equal("Yay!", e.Title);
                    Assert.Equal(new DateOnly(2022, 7, 5), e.Date);
                    Assert.Equal(1, e.OrganizerID);
                }
            );
        }
    }
}
