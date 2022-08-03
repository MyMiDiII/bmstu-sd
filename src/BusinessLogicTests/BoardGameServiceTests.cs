using Xunit;
using Moq;
using System;
using System.Linq;
using System.Collections.Generic;

using BusinessLogic.IRepositories;
using BusinessLogic.Models;
using BusinessLogic.Services;
using BusinessLogic.Exceptions;

namespace BusinessLogicTests
{
    public class BoardGameServiceTests
    {
        private readonly IBoardGameRepository _mockBGRepo;
        private readonly IBoardGameService _boardGameService;
        private readonly IPlayerService _playerService;

        private readonly List<BoardGame> _mockBoardGames;
        private readonly List<EventGame> _mockEventGames;
        private readonly List<BoardGameEvent> _mockBGEvents;
        private readonly List<FavoriteBoardGame> _mockFavoriteGames;

        public BoardGameServiceTests()
        {
            _mockBoardGames = new List<BoardGame>
            {
                new BoardGame("Title1")
                {
                    ID = 1,
                    Producer = "Producer1",
                    Year = 2001,
                    MaxAge = 5,
                    MinAge = 0,
                    MaxPlayerNum = 3,
                    MinPlayerNum = 2,
                    MaxDuration = 15,
                    MinDuration = 10
                },
                new BoardGame("Title2")
                {
                    ID = 2,
                    Producer = "Producer2",
                    Year = 2011,
                    MaxAge = 99,
                    MinAge = 12,
                    MaxPlayerNum = 20,
                    MinPlayerNum = 5,
                    MaxDuration = 180,
                    MinDuration = 60
                },
                new BoardGame("Title1")
                {
                    ID = 3,
                    Producer = "Producer2",
                    Year = 2001,
                    MaxAge = 18,
                    MinAge = 12,
                    MaxPlayerNum = 8,
                    MinPlayerNum = 2,
                    MaxDuration = 15,
                    MinDuration = 10
                }
            };
            _mockEventGames = new List<EventGame> { new EventGame(1, 1) };
            _mockBGEvents = new List<BoardGameEvent>
            {
                new BoardGameEvent("First", new DateOnly(2001, 1, 1)) { ID = 1 },
                new BoardGameEvent("Second", new DateOnly(2001, 1, 2)) { ID = 2 }
            };
            _mockFavoriteGames = new List<FavoriteBoardGame>() { new FavoriteBoardGame(2, 1) };

            var mockBGRepo = new Mock<IBoardGameRepository>();
            mockBGRepo.Setup(repo => repo.GetAll()).Returns(_mockBoardGames);
            mockBGRepo.Setup(repo => repo.Add(It.IsAny<BoardGame>())).Callback(
                (BoardGame boardGame) =>
                {
                    boardGame.ID = _mockBoardGames.Count + 1;
                    _mockBoardGames.Add(boardGame);
                }
                );
            mockBGRepo.Setup(repo => repo.GetByID(It.IsAny<long>())).Returns(
                (long id) => _mockBoardGames.Find(x => x.ID == id));
            mockBGRepo.Setup(repo => repo.Update(It.IsAny<BoardGame>())).Callback(
                (BoardGame boardGame) =>
                {
                    _mockBoardGames
                        .FindAll(x => x.ID == boardGame.ID)
                        .ForEach(x =>
                        {
                            x.Title = boardGame.Title;
                            x.Producer = boardGame.Producer;
                            x.Year = boardGame.Year;
                            x.MaxAge = boardGame.MaxAge;
                            x.MinAge = boardGame.MinAge;
                            x.MaxPlayerNum = boardGame.MaxPlayerNum;
                            x.MinPlayerNum = boardGame.MinPlayerNum;
                            x.MaxDuration = boardGame.MaxDuration;
                            x.MinDuration = boardGame.MinDuration;
                        });
                }
                );
            mockBGRepo.Setup(repo => repo.Delete(It.IsAny<BoardGame>())).Callback(
                (BoardGame boardGame) => _mockBoardGames.RemoveAll(x => x.ID == boardGame.ID));
            mockBGRepo.Setup(repo => repo.GetGameEvents(It.IsAny<long>())).Returns(
                (long gameID) =>
                {
                    var eventsIDs = _mockEventGames
                                    .FindAll(x => x.BoardGameID == gameID)
                                    .Select(x => x.BoardGameEventID);
                    return _mockBGEvents.FindAll(x => eventsIDs.Contains(x.ID));
                }
                );
            mockBGRepo.Setup(repo => repo.CheckGameInFavorites(It.IsAny<long>(), It.IsAny<long>())).Returns(
                (long gameID, long playerID) =>
                {
                    return _mockFavoriteGames.Where(x => x.BoardGameID == gameID
                                              && x.PlayerID == playerID).Any();
                });
            mockBGRepo.Setup(repo => repo.AddToFavorites(It.IsAny<long>(), It.IsAny<long>())).Callback(
                (long gameID, long playerID) =>
                {
                    var favoriteBoardGame = new FavoriteBoardGame(gameID, playerID);
                    _mockFavoriteGames.Add(favoriteBoardGame);
                }
                );
            mockBGRepo.Setup(repo => repo.DeleteFromFavorites(It.IsAny<long>(), It.IsAny<long>())).Callback(
                (long gameID, long playerID) =>
                _mockFavoriteGames.RemoveAll(x => x.PlayerID == playerID
                                               && x.BoardGameID == gameID));
            mockBGRepo.Setup(repo => repo.AddToEvent(It.IsAny<long>(), It.IsAny<long>())).Callback(
                (long gameID, long eventID) =>
                {
                    var eventBoardGame = new EventGame(gameID, eventID);
                    _mockEventGames.Add(eventBoardGame);
                }
                );
            mockBGRepo.Setup(repo => repo.DeleteFromEvent(It.IsAny<long>(), It.IsAny<long>())).Callback(
                (long gameID, long eventID) =>
                _mockEventGames.RemoveAll(x => x.BoardGameEventID == eventID
                                            && x.BoardGameID == gameID));
            mockBGRepo.Setup(repo => repo.CheckGamePlaying(It.IsAny<long>(), It.IsAny<long>())).Returns(
                (long gameID, long eventID) =>
                {
                    return _mockEventGames.Where(x => x.BoardGameID == gameID
                                                   && x.BoardGameEventID == eventID).Any();
                });


            _mockBGRepo = mockBGRepo.Object;

            var mockUserRepo = new Mock<IUserRepository>();
            mockUserRepo.Setup(repo => repo.GetDefaultUser()).Returns(
                new User("test", "123") { Roles = new List<Role> { new Role("player") { RoleID = 1 } } });
            var userService = new UserService(mockUserRepo.Object, new BCryptEntryptionService());

            _playerService = new PlayerService(new Mock<IPlayerRepository>().Object, userService);
            _boardGameService = new BoardGameService(_mockBGRepo, _playerService);
        }

        [Fact]
        public void GetBoardGamesTest()
        {
            var expectedCount = _mockBoardGames.Count;

            var res = _boardGameService.GetBoardGames();

            Assert.IsType<List<BoardGame>>(res);
            Assert.Equal(expectedCount, res.Count);
            Assert.All(res, item => Assert.InRange(item.ID, low: 1, high: expectedCount));
        }

        [Fact]
        public void CreateBoardGameTest()
        {
            var expectedCount = _mockBoardGames.Count;
            var expectedCount2 = expectedCount + 1;
            var res = _boardGameService.GetBoardGames();
            var boardGame = new BoardGame("Пандемия")
            {
                Producer = "Hobby World",
                Year = 2014,
                MaxAge = 99,
                MinAge = 12,
                MaxPlayerNum = 4,
                MinPlayerNum = 1,
                MaxDuration = 90,
                MinDuration = 40
            };

            Assert.Equal(expectedCount, res.Count);

            _boardGameService.CreateBoardGame(boardGame);

            res = _boardGameService.GetBoardGames();

            Assert.Equal(expectedCount2, res.Count);
            Assert.All(res, item => Assert.InRange(item.ID, low: 1, high: expectedCount2));
        }

        [Fact]
        public void ThrowAlreadyExistsExcCreateBoardGameTest()
        {
            var boardGame = new BoardGame("Title1")
            {
                Producer = "Producer1",
                Year = 2001,
            };

            void action() => _boardGameService.CreateBoardGame(boardGame);

            Assert.Throws<AlreadyExistsBoardGameException>(action);
        }

        [Fact]
        public void UpdateBoardGameTest()
        {
            var expectedCount = _mockBoardGames.Count;
            var boardGame = new BoardGame("Бункер")
            {
                ID = 1,
                Producer = "Экономикус",
                Year = 2001,
                MaxAge = 99,
                MinAge = 18,
                MaxPlayerNum = 16,
                MinPlayerNum = 4,
                MaxDuration = 60,
                MinDuration = 30
            };

            var res = _boardGameService.GetBoardGames();
            Assert.Equal(expectedCount, res.Count);

            _boardGameService.UpdateBoardGame(boardGame);

            res = _boardGameService.GetBoardGames();

            Assert.Equal(expectedCount, res.Count);
            Assert.All(res, item => Assert.InRange(item.ID, low: 1, high: expectedCount));
            var newVal = res.Find(item => item.ID == boardGame.ID);
            Assert.Equal(newVal?.ID, boardGame.ID);
            Assert.Equal(newVal?.Title, boardGame.Title);
            Assert.Equal(newVal?.Producer, boardGame.Producer);
            Assert.Equal(newVal?.Year, boardGame.Year);
            Assert.Equal(newVal?.MinAge, boardGame.MinAge);
            Assert.Equal(newVal?.MaxAge, boardGame.MaxAge);
            Assert.Equal(newVal?.MinPlayerNum, boardGame.MinPlayerNum);
            Assert.Equal(newVal?.MaxPlayerNum, boardGame.MaxPlayerNum);
            Assert.Equal(newVal?.MinDuration, boardGame.MinDuration);
            Assert.Equal(newVal?.MaxDuration, boardGame.MaxDuration);
        }

        [Fact]
        public void ThrowNotExistsExcUpdateBoardGameTest()
        {
            var boardGame = new BoardGame("1") { ID = 100 };

            void action() => _boardGameService.UpdateBoardGame(boardGame);

            Assert.Throws<NotExistsBoardGameException>(action);
        }

        [Fact]
        public void DeleteBoardGameTest()
        {
            var expectedCount = _mockBoardGames.Count;
            var boardGame = new BoardGame("1") { ID = 1 };

            var res = _boardGameService.GetBoardGames();
            Assert.Equal(expectedCount, res.Count);

            _boardGameService.DeleteBoardGame(boardGame);

            res = _boardGameService.GetBoardGames();

            Assert.Equal(expectedCount - 1, res.Count);
            Assert.Null(_mockBoardGames.Find(x => x.ID == boardGame.ID));
        }

        [Fact]
        public void ThrowNotExistsExcDeleteBoardGameTest()
        {
            var boardGame = new BoardGame("1") { ID = 100 };

            void action() => _boardGameService.DeleteBoardGame(boardGame);

            Assert.Throws<NotExistsBoardGameException>(action);
        }

        [Fact]
        public void AddBoardGameToFavoriteTest()
        {
            var expectedCount = _mockFavoriteGames.Count + 1;

            var game = new BoardGame("123") { ID = 1 };

            _boardGameService.AddBoardGameToFavorite(game);

            Assert.Equal(expectedCount, _mockFavoriteGames.Count);
            Assert.NotNull(_mockFavoriteGames.Find(x => x.PlayerID == 1
                                                     && x.BoardGameID == game.ID));
        }

        [Fact]
        public void DeleteBoardGameFromFavoriteTest()
        {
            _mockFavoriteGames.Add(new FavoriteBoardGame(1, 1));
            var expectedCount = _mockFavoriteGames.Count - 1;

            var game = new BoardGame("123") { ID = 1 };

            _boardGameService.DeleteBoardGameFromFavorite(game);

            Assert.Equal(expectedCount, _mockFavoriteGames.Count);
        }

        [Fact]
        public void GetEventsByGameTest()
        {
            var expectedCount = 1;
            var game = new BoardGame("1") { ID = 1 };

            var events = _boardGameService.GetEventsByGame(game);

            Assert.Equal(expectedCount, events.Count);
            Assert.Equal("First", events.First().Title);
        }

        [Fact]
        public void AddBoardGameToEventTest()
        {
            var expectedCount = _mockEventGames.Count + 1;

            var game = _mockBoardGames.First();
            var bgEvent = _mockBGEvents.Last();

            _boardGameService.AddBoardGameToEvent(game, bgEvent);

            Assert.Equal(expectedCount, _mockEventGames.Count);
            Assert.NotNull(_mockEventGames.Find(x => x.BoardGameEventID == bgEvent.ID
                                                  && x.BoardGameID == game.ID));
        }

        [Fact]
        public void DeleteBoardGameFromEventTest()
        {
            var expectedCount = _mockEventGames.Count - 1;

            var game = _mockBoardGames.First();
            var bgEvent = _mockBGEvents.First();

            _boardGameService.DeleteBoardGameFromEvent(game, bgEvent);

            Assert.Equal(expectedCount, _mockEventGames.Count);
        }
    }
}
