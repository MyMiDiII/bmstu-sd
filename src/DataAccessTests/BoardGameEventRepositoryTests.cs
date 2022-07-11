using Xunit;
using Microsoft.EntityFrameworkCore;

using DataAccess;
using DataAccess.Repositories;
using BusinessLogic.Models;

namespace DataAccessTests
{
    public class BoardGameEventRepositoryTests
    {
        private readonly DbContextOptions<BGEContext> _dbContextOptions;

        public BoardGameEventRepositoryTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<BGEContext>()
                .UseInMemoryDatabase("BoardGameEventTestDB")
                .Options;

            using var context = new BGEContext(_dbContextOptions);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.Games.AddRange(new BoardGame("Игра"),
                                   new BoardGame("Game"));
            context.Events.AddRange(
                new BoardGameEvent("Yay!", new DateOnly(2022, 7, 5))
                {
                    StartTime = new TimeOnly(19, 30),
                    Duration = 90,
                    Purchase = false,
                    State = BoardGameEventState.Finished
                },
                new BoardGameEvent("No!", new DateOnly(2022, 7, 7))
                {
                    StartTime = new TimeOnly(11, 00),
                    Duration = 300,
                    Purchase = true,
                    State = BoardGameEventState.Cancelled
                }
            );
            context.EventGameRelations.AddRange(new EventGame(1, 1), new EventGame(2, 2));

            context.Players.AddRange(new Player("MyMiDi"), new Player("hamzreg"));
            context.Registrations.AddRange(new PlayerRegistration(2, 1), new PlayerRegistration(1, 2));

            context.SaveChanges();
        }

        private BGEContext CreateContext() => new(_dbContextOptions);
        private BoardGameEventRepository CreateBoardGameEventRepository() => new(CreateContext());
        private static BoardGameEventRepository CreateBoardGameEventRepository(BGEContext context) =>
            new(context);

        [Fact]
        public void BoardGameEventGetAllTest()
        {
            var rep = CreateBoardGameEventRepository();

            var events = rep.GetAll();

            Assert.Equal(2, events.Count);
            Assert.Collection(
                events,
                e =>
                {
                    Assert.Equal(1, e.ID);
                    Assert.Equal("Yay!", e.Title);
                    Assert.Equal(new DateOnly(2022, 7, 5), e.Date);
                },
                e =>
                {
                    Assert.Equal(2, e.ID);
                    Assert.Equal("No!", e.Title);
                    Assert.Equal(new DateOnly(2022, 7, 7), e.Date);
                }
            );
        }

        [Fact]
        public void BoardGameEventGetByIDTest()
        {
            var rep = CreateBoardGameEventRepository();

            var bgEvent = rep.GetByID(1);

            Assert.NotNull(bgEvent);
            Assert.Equal(1, bgEvent?.ID);
            Assert.Equal("Yay!", bgEvent?.Title);
            Assert.Equal(new DateOnly(2022, 7, 5), bgEvent?.Date);
        }

        [Fact]
        public void BoardGameEventAddTest()
        {
            var context = CreateContext();
            var rep = CreateBoardGameEventRepository(context);
            var bgEvent = new BoardGameEvent("Играй!", new DateOnly(2022, 7, 9));

            rep.Add(bgEvent);

            Assert.Equal(3, context.Events.Count());
            var added = context.Events.Single(g => g.Title == "Играй!");
            Assert.NotNull(added);
            Assert.Equal(3, added?.ID);
            Assert.Equal("Играй!", added?.Title);
            Assert.Equal(new DateOnly(2022, 7, 9), bgEvent?.Date);
        }

        [Fact]
        public void BoardGameEventUpdateTest()
        {
            var context = CreateContext();
            var rep = CreateBoardGameEventRepository(context);
            var bgEvent = new BoardGameEvent("Играй!", new DateOnly(2022, 7, 9)) { ID = 2 };

            rep.Update(bgEvent);

            Assert.Equal(2, context.Events.Count());
            Assert.Null(context.Events.FirstOrDefault(g => g.Title == "No!"));
            var updated = context.Events.Single(g => g.Title== "Играй!");
            Assert.NotNull(updated);
            Assert.Equal(2, updated?.ID);
            Assert.Equal("Играй!", updated?.Title);
            Assert.Equal(new DateOnly(2022, 7, 9), updated?.Date);
        }

        [Fact]
        public void BoardGameEventDeleteTest()
        {
            var context = CreateContext();
            var rep = CreateBoardGameEventRepository(context);
            var bgEvent = new BoardGameEvent("No!", new DateOnly(2022, 7, 7)) { ID = 2 };

            rep.Delete(bgEvent);

            Assert.Equal(2, context.Events.Count());
            var deleted = context.Events.Single(g => g.Title== "No!");
            Assert.NotNull(deleted);
            Assert.True(deleted.Deleted);
            Assert.Equal(2, deleted?.ID);
            Assert.Equal("No!", deleted?.Title);
            Assert.Equal(new DateOnly(2022, 7, 7), deleted?.Date);
        }

        [Fact]
        public void BoardGameEventGetByTitleTest()
        {
            var rep = CreateBoardGameEventRepository();

            var events = rep.GetByTitle("ya");

            Assert.NotNull(events);
            Assert.NotEmpty(events);
            var found = events.FirstOrDefault(g => g.Title== "Yay!");
            Assert.NotNull(found);
            Assert.Equal(1, found?.ID);
            Assert.Equal("Yay!", found?.Title);
            Assert.Equal(new DateOnly(2022, 7, 5), found?.Date);
        }

        [Fact]
        public void BoardGameEventGetByDateTest()
        {
            var rep = CreateBoardGameEventRepository();

            var events = rep.GetByDate(new DateOnly(2022, 7, 7));

            Assert.Single(events);
            Assert.Collection(
                events,
                e =>
                {
                    Assert.Equal(2, e.ID);
                    Assert.Equal("No!", e.Title);
                    Assert.Equal(new DateOnly(2022, 7, 7), e.Date);
                }
            );
        }

        [Fact]
        public void BoardGameEventGetByStartTimeTest()
        {
            var rep = CreateBoardGameEventRepository();

            var events = rep.GetByStartTime(new TimeOnly(19, 30));

            Assert.Single(events);
            Assert.Collection(
                events,
                e =>
                {
                    Assert.Equal(1, e.ID);
                    Assert.Equal("Yay!", e.Title);
                    Assert.Equal(new DateOnly(2022, 7, 5), e.Date);
                    Assert.Equal(new TimeOnly(19, 30), e.StartTime);
                }
            );
        }

        [Fact]
        public void BoardGameEventGetByDurationTest()
        {
            var rep = CreateBoardGameEventRepository();

            var events = rep.GetByDuration(240);

            Assert.Single(events);
            Assert.Collection(
                events,
                e =>
                {
                    Assert.Equal(1, e.ID);
                    Assert.Equal("Yay!", e.Title);
                    Assert.Equal(new DateOnly(2022, 7, 5), e.Date);
                    Assert.True(240 >= e.Duration);
                }
            );
        }

        [Fact]
        public void BoardGameEventGetByPurchaseTest()
        {
            var rep = CreateBoardGameEventRepository();

            var events = rep.GetByPurchase(true);

            Assert.Single(events);
            Assert.Collection(
                events,
                e =>
                {
                    Assert.Equal(2, e.ID);
                    Assert.Equal("No!", e.Title);
                    Assert.Equal(new DateOnly(2022, 7, 7), e.Date);
                    Assert.True(e.Purchase);
                }
            );
        }

        [Fact]
        public void BoardGameEventGetByStateTest()
        {
            var rep = CreateBoardGameEventRepository();

            var events = rep.GetByState(BoardGameEventState.Finished);

            Assert.Single(events);
            Assert.Collection(
                events,
                e =>
                {
                    Assert.Equal(1, e.ID);
                    Assert.Equal("Yay!", e.Title);
                    Assert.Equal(new DateOnly(2022, 7, 5), e.Date);
                    Assert.Equal(BoardGameEventState.Finished, e.State);
                }
            );
        }

        [Fact]
        public void BoardGameEventGetGamesTest()
        {
            var context = CreateContext();
            var rep = CreateBoardGameEventRepository(context);

            var games = rep.GetEventGames(1);

            Assert.Single(games);
            Assert.Collection(
                games,
                e =>
                {
                    Assert.Equal(1, e.ID);
                    Assert.Equal("Игра", e.Title);
                }
            );
        }

        [Fact]
        public void BoardGameEventGetPlayersTest()
        {
            var context = CreateContext();
            var rep = CreateBoardGameEventRepository(context);

            var players = rep.GetEventPlayers(1);

            Assert.Single(players);
            Assert.Collection(
                players,
                p =>
                {
                    Assert.Equal(2, p.ID);
                    Assert.Equal("hamzreg", p.Name);
                }
            );
        }
    }
}
