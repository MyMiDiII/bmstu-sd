using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

using DataAccess;
using DataAccess.Repositories;
using BusinessLogic.Models;

namespace DataAccessTests
{
    public class VenueRepositoryTests
    {
        private readonly DbContextOptions<BGEContext> _dbContextOptions;

        public VenueRepositoryTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<BGEContext>()
                .UseInMemoryDatabase("VenueTestDB")
                .Options;

            using var context = new BGEContext(_dbContextOptions);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.Venues.AddRange(new Venue("V1", "антикафе", "Москва"));

            context.SaveChanges();
        }

        private BGEContext CreateContext() => new BGEContext(_dbContextOptions);
        private VenueRepository CreateVenueRepository() => new VenueRepository(CreateContext());
        private VenueRepository CreateVenueRepository(BGEContext context) =>
            new VenueRepository(context);

        [Fact]
        public void VenueGetAllTest()
        {
            var rep = CreateVenueRepository();
            var venues = rep.GetAll();

            Assert.Single(venues);
            Assert.Collection(
                venues,
                v =>
                {
                    Assert.Equal(1, v.ID);
                    Assert.Equal("V1", v.Name);
                    Assert.Equal("антикафе", v.Type);
                    Assert.Equal("Москва", v.Address);
                }
            );
        }

        [Fact]
        public void VenueGetByIDTest()
        {
            var rep = CreateVenueRepository();
            var venue = rep.GetByID(1);

            Assert.NotNull(venue);
            Assert.Equal(1, venue?.ID);
            Assert.Equal("V1", venue?.Name);
            Assert.Equal("антикафе", venue?.Type);
            Assert.Equal("Москва", venue?.Address);
        }

        [Fact]
        public void VenueAddTest()
        {
            var context = CreateContext();
            var rep = CreateVenueRepository(context);
            var venue = new Venue("V2", "кафе", "Орел");

            rep.Add(venue);

            Assert.Equal(2, context.Venues.Count());
            var added = context.Venues.Single(v => v.Name == "V2");
            Assert.NotNull(added);
            Assert.Equal(2, added?.ID);
            Assert.Equal("V2", added?.Name);
            Assert.Equal("кафе", added?.Type);
            Assert.Equal("Орел", added?.Address);
        }

        [Fact]
        public void VenueUpdateTest()
        {
            var context = CreateContext();
            var rep = CreateVenueRepository(context);
            var venue = new Venue("Лучшее", "кафе", "Москва") { ID = 1 };

            rep.Update(venue);

            Assert.Single(context.Venues);
            Assert.Null(context.Venues.FirstOrDefault(v => v.Name == "V1"));
            var updated = context.Venues.Single(v => v.Name == "Лучшее");
            Assert.NotNull(updated);
            Assert.Equal(1, updated?.ID);
            Assert.Equal("Лучшее", updated?.Name);
            Assert.Equal("кафе", updated?.Type);
            Assert.Equal("Москва", updated?.Address);
        }

        [Fact]
        public void VenueGetByTypeTest()
        {
            var rep = CreateVenueRepository();

            var venues = rep.GetByType("КаФе");

            Assert.NotNull(venues);
            Assert.NotEmpty(venues);
            var found = venues.FirstOrDefault(v => v.Name == "V1");
            Assert.NotNull(found);
            Assert.Equal(1, found?.ID);
            Assert.Equal("V1", found?.Name);
            Assert.Equal("антикафе", found?.Type);
            Assert.Equal("Москва", found?.Address);
        }
    }
}