﻿using Xunit;
using Microsoft.EntityFrameworkCore;

using DataAccess;
using DataAccess.Repositories;
using BusinessLogic.Models;

namespace DataAccessTests
{
    public class BoardGameRepositoryTests
    {
        private readonly DbContextOptions<BGEContext> _dbContextOptions;

        public BoardGameRepositoryTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<BGEContext>()
                .UseInMemoryDatabase("BoardGameTestDB")
                .Options;

            using var context = new BGEContext(_dbContextOptions);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.Games.AddRange(new BoardGame("Игра") { Year = 2002, MinAge = 0, MaxAge = 10 },
                                   new BoardGame("Game") { Year = 2001, MinAge = 5, MaxAge = 12 });
            context.Events.AddRange(new BoardGameEvent("Yay!", new DateOnly(2022, 7, 5)),
                                    new BoardGameEvent("No!", new DateOnly(2022, 7, 7)));
            context.EventGameRelations.AddRange(new EventGame(1, 1), new EventGame(2, 2)); 

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

            var games = rep.GetByTitle("игра");

            Assert.NotNull(games);
            Assert.NotEmpty(games);
            var found = games.FirstOrDefault(g => g.Title== "Игра");
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
    }
}
