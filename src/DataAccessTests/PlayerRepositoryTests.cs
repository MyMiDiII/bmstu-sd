using System.Data.Common;

using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;

using DataAccess;
using DataAccess.Repositories;
using BusinessLogic.Models;
using BusinessLogic.Exceptions;

namespace DataAccessTests
{
    public class PlayerRepositoryTests
    {
        private readonly DbConnection _dbconnection;
        private readonly DbContextOptions<BGEContext> _dbContextOptions;

        public PlayerRepositoryTests()
        {
            _dbconnection = new SqliteConnection("Filename=:memory:");
            _dbconnection.Open();

            _dbContextOptions = new DbContextOptionsBuilder<BGEContext>()
                .UseSqlite(_dbconnection)
                .Options;

            using var context = new BGEContext(_dbContextOptions);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.Games.AddRange(new BoardGame("Игра") { Year = 2002, MinAge = 0, MaxAge = 10 },
                                   new BoardGame("Game") { Year = 2001, MinAge = 5, MaxAge = 12 });
            context.Events.AddRange(new BoardGameEvent("Yay!", new DateOnly(2022, 7, 5)),
                                    new BoardGameEvent("No!", new DateOnly(2022, 7, 7)));
            context.Players.AddRange(new Player("MyMiDi") { League = "Бывалый", Rating = 1 },
                                     new Player("???") { League = "Просто зашел", Rating = 10} );
            context.Favorites.AddRange(new FavoriteBoardGame(1, 1), new FavoriteBoardGame(2, 2));
            context.Registrations.AddRange(new PlayerRegistration(1, 1), new PlayerRegistration(2, 2));

            context.SaveChanges();
        }

        private BGEContext CreateContext() => new(_dbContextOptions);
        private PlayerRepository CreatePlayerRepository() => new(CreateContext());
        private static PlayerRepository CreatePlayerRepository(BGEContext context) =>
            new(context);

        [Fact]
        public void PlayerGetAllTest()
        {
            var rep = CreatePlayerRepository();
            var players = rep.GetAll();

            Assert.Equal(2, players.Count);
            Assert.Collection(
                players,
                p =>
                {
                    Assert.Equal(1, p.ID);
                    Assert.Equal("MyMiDi", p.Name);
                },
                p =>
                {
                    Assert.Equal(2, p.ID);
                    Assert.Equal("???", p.Name);
                }
            );
        }

        [Fact]
        public void PlayerGetByIDTest()
        {
            var rep = CreatePlayerRepository();
            var player = rep.GetByID(2);

            Assert.NotNull(player);
            Assert.Equal(2, player?.ID);
            Assert.Equal("???", player?.Name);
        }

        [Fact]
        public void PlayerAddTest()
        {
            var context = CreateContext();
            var rep = CreatePlayerRepository(context);
            var player = new Player("111");

            rep.Add(player);

            Assert.Equal(3, context.Players.Count());
            var added = context.Players.Single(p => p.Name == "111");
            Assert.NotNull(added);
            Assert.Equal(3, added?.ID);
            Assert.Equal("111", added?.Name);
        }

        [Fact]
        public void PlayerUpdateTest()
        {
            var context = CreateContext();
            var rep = CreatePlayerRepository(context);
            var player = new Player("Манчкин123") { ID = 2 };

            rep.Update(player);

            Assert.Equal(2, context.Players.Count());
            Assert.Null(context.Players.FirstOrDefault(p => p.Name == "Game"));
            var updated = context.Players.Single(p => p.Name == "Манчкин123");
            Assert.NotNull(updated);
            Assert.Equal(2, updated?.ID);
            Assert.Equal("Манчкин123", updated?.Name);
        }

        [Fact]
        public void PlayerDeleteTest()
        {
            var context = CreateContext();
            var rep = CreatePlayerRepository(context);
            var player = new Player("???") { ID = 2 };

            rep.Delete(player);

            Assert.Equal(2, context.Players.Count());
            var deleted = context.Players.Single(g => g.Name == "???");
            Assert.NotNull(deleted);
            Assert.True(deleted.Deleted);
            Assert.Equal(2, deleted?.ID);
            Assert.Equal("???", deleted?.Name);
        }

        [Fact]
        public void PlayerGetByTitleTest()
        {
            var rep = CreatePlayerRepository();

            var player = rep.GetByName("???");

            Assert.NotNull(player);
            Assert.Equal(2, player?.ID);
            Assert.Equal("???", player?.Name);
        }

        [Fact]
        public void PlayerGetByLeagueTest()
        {
            var rep = CreatePlayerRepository();

            var players = rep.GetByLeague("Бывалый");

            Assert.Single(players);
            Assert.Collection(
                players,
                p =>
                {
                    Assert.Equal(1, p.ID);
                    Assert.Equal("MyMiDi", p.Name);
                }
            );
        }

        [Fact]
        public void PlayerGetByRatingTest()
        {
            var rep = CreatePlayerRepository();

            var players = rep.GetByRating(10);

            Assert.Single(players);
            Assert.Collection(
                players,
                p =>
                {
                    Assert.Equal(2, p.ID);
                    Assert.Equal("???", p.Name);
                }
            );
        }

        [Fact]
        public void PlayerAddToEventTest()
        {
            var context = CreateContext();
            var rep = CreatePlayerRepository(context);

            rep.AddToEvent(2, 1);

            Assert.Equal(3, context.Registrations.Count());
            var added = context.Registrations
                        .FirstOrDefault(egr => egr.PlayerID == 2 && egr.BoardGameEventID == 1);
            Assert.NotNull(added);
        }

        [Fact]
        public void PlayerAddToEventExceptionTest()
        {
            var context = CreateContext();
            var rep = CreatePlayerRepository(context);

            void action() => rep.AddToEvent(1, 1);

            Assert.Throws<AlreadyExistsEventGameException>(action);
        }

        [Fact]
        public void PlayerDeleteFromEventTest()
        {
            var context = CreateContext();
            var rep = CreatePlayerRepository(context);

            rep.DeleteFromEvent(1, 1);

            Assert.Equal(1, context.Registrations.Count());
            var deleted = context.Registrations.FirstOrDefault(x => x.PlayerID == 1
                                                                 && x.BoardGameEventID == 1);
            Assert.Null(deleted);
        }

        [Fact]
        public void PlayerGetEventsTest()
        {
            var context = CreateContext();
            var rep = CreatePlayerRepository(context);

            var events = rep.GetPlayerEvents(1);

            Assert.Single(events);
            Assert.Collection(
                events,
                e =>
                {
                    Assert.Equal(1, e.ID);
                    Assert.Equal("Yay!", e.Title);
                    Assert.Equal(new DateOnly(2022, 7, 5), e.Date);
                }
            );
        }

        [Fact]
        public void PlayerGetFavoritesTest()
        {
            var context = CreateContext();
            var rep = CreatePlayerRepository(context);

            var games = rep.GetPlayerFavorites(1);

            Assert.Single(games);
            Assert.Collection(
                games,
                g =>
                {
                    Assert.Equal(1, g.ID);
                    Assert.Equal("Игра", g.Title);
                }
            );
        }
    }
}
