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
    public class BoardGameRepositoryTests
    {
        private readonly DbConnection _dbconnection;
        private readonly DbContextOptions<BGEContext> _dbContextOptions;

        public BoardGameRepositoryTests()
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
            context.Games.AddRange(new BoardGame("Игра") { Year = 2002, MinAge = 0, MaxAge = 10 },
                                   new BoardGame("Game") { Year = 2001, MinAge = 5, MaxAge = 12 });
            context.Players.AddRange(new Player("MyMiDi") { League = "Бывалый", Rating = 1 },
                                     new Player("???") { League = "Просто зашел", Rating = 10} );
            context.SaveChanges();

            context.Events.AddRange(new BoardGameEvent("Yay!", new DateOnly(2022, 7, 5))
                                                       { OrganizerID = 1, VenueID = 1 },
                                    new BoardGameEvent("No!", new DateOnly(2022, 7, 7))
                                                       { OrganizerID = 1, VenueID = 1 });
            context.SaveChanges();

            context.EventGameRelations.AddRange(new EventGame(1, 1), new EventGame(2, 2)); 
            context.Favorites.AddRange(new FavoriteBoardGame(1, 1), new FavoriteBoardGame(2, 2));
            context.SaveChanges();
        }

        private BGEContext CreateContext() => new(_dbContextOptions);
        private BoardGameRepository CreateBoardGameRepository() => new(CreateContext());
        private static BoardGameRepository CreateBoardGameRepository(BGEContext context) =>
            new(context);

        [Fact]
        public void BoardGameGetAllTest()
        {
            var rep = CreateBoardGameRepository();
            var games = rep.GetAll();

            Assert.Equal(2, games.Count);
            Assert.Collection(
                games,
                g =>
                {
                    Assert.Equal(1, g.ID);
                    Assert.Equal("Игра", g.Title);
                },
                g =>
                {
                    Assert.Equal(2, g.ID);
                    Assert.Equal("Game", g.Title);
                }
            );
        }

        [Fact]
        public void BoardGameGetByIDTest()
        {
            var rep = CreateBoardGameRepository();
            var game = rep.GetByID(2);

            Assert.NotNull(game);
            Assert.Equal(2, game?.ID);
            Assert.Equal("Game", game?.Title);
        }

        [Fact]
        public void BoardGameAddTest()
        {
            var context = CreateContext();
            var rep = CreateBoardGameRepository(context);
            var game = new BoardGame("Red7");

            rep.Add(game);

            Assert.Equal(3, context.Games.Count());
            var added = context.Games.Single(g => g.Title == "Red7");
            Assert.NotNull(added);
            Assert.Equal(3, added?.ID);
            Assert.Equal("Red7", added?.Title);
        }

        [Fact]
        public void BoardGameUpdateTest()
        {
            var context = CreateContext();
            var rep = CreateBoardGameRepository(context);
            var game = new BoardGame("Манчкин") { ID = 2 };

            rep.Update(game);

            Assert.Equal(2, context.Games.Count());
            Assert.Null(context.Games.FirstOrDefault(g => g.Title == "Game"));
            var updated = context.Games.Single(g => g.Title== "Манчкин");
            Assert.NotNull(updated);
            Assert.Equal(2, updated?.ID);
            Assert.Equal("Манчкин", updated?.Title);
        }

        [Fact]
        public void BoardGameDeleteTest()
        {
            var context = CreateContext();
            var rep = CreateBoardGameRepository(context);
            var game = new BoardGame("Игра") { ID = 1 };

            rep.Delete(game);

            Assert.Equal(2, context.Games.Count());
            var deleted = context.Games.Single(g => g.Title== "Игра");
            Assert.NotNull(deleted);
            Assert.True(deleted.Deleted);
            Assert.Equal(1, deleted?.ID);
            Assert.Equal("Игра", deleted?.Title);
        }

        [Fact]
        public void BoardGameGetByTitleTest()
        {
            var rep = CreateBoardGameRepository();

            var games = rep.GetByTitle("Игра");

            Assert.NotNull(games);
            Assert.NotEmpty(games);
            var found = games.FirstOrDefault(g => g.Title == "Игра");
            Assert.NotNull(found);
            Assert.Equal(1, found?.ID);
            Assert.Equal("Игра", found?.Title);
        }

        [Fact]
        public void BoardGameGetByYearTest()
        {
            var rep = CreateBoardGameRepository();

            var games = rep.GetByYear(2001);

            Assert.Single(games);
            Assert.Collection(
                games,
                g =>
                {
                    Assert.Equal(2, g.ID);
                    Assert.Equal("Game", g.Title);
                }
            );
        }

        [Fact]
        public void BoardGameGetByAgeTest()
        {
            var rep = CreateBoardGameRepository();

            var games = rep.GetByAge(5, 13);

            Assert.Single(games);
            Assert.Collection(
                games,
                g =>
                {
                    Assert.Equal(2, g.ID);
                    Assert.Equal("Game", g.Title);
                }
            );
        }

        [Fact]
        public void BoardGameAddToEventTest()
        {
            var context = CreateContext();
            var rep = CreateBoardGameRepository(context);

            rep.AddToEvent(2, 1);

            Assert.Equal(3, context.EventGameRelations.Count());
            var added = context.EventGameRelations
                        .FirstOrDefault(egr => egr.BoardGameID == 2 && egr.BoardGameEventID == 1);
            Assert.NotNull(added);
        }

        [Fact]
        public void BoardGameAddToEventExceptionTest()
        {
            var context = CreateContext();
            var rep = CreateBoardGameRepository(context);

            void action() => rep.AddToEvent(1, 1);

            Assert.Throws<AlreadyExistsEventGameException>(action);
        }

        [Fact]
        public void BoardGameDeleteFromEventTest()
        {
            var context = CreateContext();
            var rep = CreateBoardGameRepository(context);

            rep.DeleteFromEvent(1, 1);

            Assert.Equal(1, context.EventGameRelations.Count());
            var deleted = context.EventGameRelations.FirstOrDefault(x => x.BoardGameID == 1
                                                                      && x.BoardGameEventID == 1);
            Assert.Null(deleted);
        }

        [Fact]
        public void BoardGameGetEventsTest()
        {
            var context = CreateContext();
            var rep = CreateBoardGameRepository(context);

            var events = rep.GetGameEvents(1);

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
        public void BoardGameAddToFavoritesTest()
        {
            var context = CreateContext();
            var rep = CreateBoardGameRepository(context);

            rep.AddToFavorites(2, 1);

            Assert.Equal(3, context.Favorites.Count());
            var added = context.Favorites
                        .FirstOrDefault(egr => egr.BoardGameID == 2 && egr.PlayerID == 1);
            Assert.NotNull(added);
        }

        [Fact]
        public void BoardGameAddToFavoritesExceptionTest()
        {
            var context = CreateContext();
            var rep = CreateBoardGameRepository(context);

            void action() => rep.AddToFavorites(1, 1);

            Assert.Throws<AlreadyExistsFavoriteGameException>(action);
        }

        [Fact]
        public void BoardGameDeleteFromFavoritesTest()
        {
            var context = CreateContext();
            var rep = CreateBoardGameRepository(context);

            rep.DeleteFromFavorites(1, 1);

            Assert.Equal(1, context.Favorites.Count());
            var deleted = context.Favorites.FirstOrDefault(x => x.BoardGameID == 1
                                                             && x.PlayerID == 1);
            Assert.Null(deleted);
        }

    }
}
