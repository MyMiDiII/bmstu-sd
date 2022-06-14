using Xunit;
using Moq;
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
        private IBoardGameRepository _mockRepo;
        private IBoardGameService _service;

        List<BoardGame> _mockBoardGames;
        List<EventGame> _mockEventGames;
        List<BoardGameEvent> _mockBGEvents;

        public BoardGameServiceTests()
        {
            _mockBoardGames = new List<BoardGame>
            {
                new BoardGame
                {
                    ID = 1,
                    Title = "Title1",
                    Produser = "Producer1",
                    Year = 2001,
                    MaxAge = 5,
                    MinAge = 0,
                    MaxPlayerNum = 3,
                    MinPlayerNum = 2,
                    MaxDuration = 15,
                    MinDuration = 10
                },
                new BoardGame
                {
                    ID = 2,
                    Title = "Title2",
                    Produser = "Producer2",
                    Year = 2011,
                    MaxAge = 99,
                    MinAge = 12,
                    MaxPlayerNum = 20,
                    MinPlayerNum = 5,
                    MaxDuration = 180,
                    MinDuration = 60
                },
                new BoardGame
                {
                    ID = 3,
                    Title = "Title1",
                    Produser = "Producer2",
                    Year = 2001,
                    MaxAge = 18,
                    MinAge = 12,
                    MaxPlayerNum = 8,
                    MinPlayerNum = 2,
                    MaxDuration = 15,
                    MinDuration = 10
                }
            };
            _mockEventGames = new List<EventGame>
            {
                new EventGame { BoardGameEventID = 1, BoardGameID = 1 }
            };
            _mockBGEvents = new List<BoardGameEvent>
            {
                new BoardGameEvent { ID = 1, Title = "First" }
            };

            var mockRepo = new Mock<IBoardGameRepository>();
            mockRepo.Setup(repo => repo.GetAll()).Returns(_mockBoardGames);
            mockRepo.Setup(repo => repo.Add(It.IsAny<BoardGame>())).Callback(
                (BoardGame boardGame) =>
                {
                    boardGame.ID = _mockBoardGames.Count + 1;
                    _mockBoardGames.Add(boardGame);
                }
                );
            mockRepo.Setup(repo => repo.GetByID(It.IsAny<long>())).Returns(
                (long id) => _mockBoardGames.Find(x => x.ID == id));
            mockRepo.Setup(repo => repo.Update(It.IsAny<BoardGame>())).Callback(
                (BoardGame boardGame) =>
                {
                    _mockBoardGames
                        .FindAll(x => x.ID == boardGame.ID)
                        .ForEach(x =>
                        {
                            x.Title = boardGame.Title;
                            x.Produser = boardGame.Produser;
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
            mockRepo.Setup(repo => repo.Delete(It.IsAny<BoardGame>())).Callback(
                (BoardGame boardGame) => _mockBoardGames.RemoveAll(x => x.ID == boardGame.ID));
            mockRepo.Setup(repo => repo.GetGameEvents(It.IsAny<long>())).Returns(
                (long gameID) =>
                {
                    var eventsIDs = _mockEventGames
                                    .FindAll(x => x.BoardGameID == gameID)
                                    .Select(x => x.BoardGameEventID);
                    return _mockBGEvents.FindAll(x => eventsIDs.Contains(x.ID));
                }
                );

            _mockRepo = mockRepo.Object;
            _service = new BoardGameService(_mockRepo);
        }

        [Fact]
        public void GetBoardGamesTest()
        {
            var expectedCount = _mockBoardGames.Count;

            var res = _service.GetBoardGames();

            Assert.IsType<List<BoardGame>>(res);
            Assert.Equal(expectedCount, res.Count);
            Assert.All(res, item => Assert.InRange(item.ID, low: 1, high: expectedCount));
        }

        [Fact]
        public void CreateBoardGameTest()
        {
            var expectedCount = _mockBoardGames.Count;
            var expectedCount2 = expectedCount + 1;
            var res = _service.GetBoardGames();
            var boardGame = new BoardGame
            {
                Title = "Пандемия",
                Produser = "Hobby World",
                Year = 2014,
                MaxAge = 99,
                MinAge = 12,
                MaxPlayerNum = 4,
                MinPlayerNum = 1,
                MaxDuration = 90,
                MinDuration = 40
            };

            Assert.Equal(expectedCount, res.Count);

            _service.CreateBoardGame(boardGame);

            res = _service.GetBoardGames();

            Assert.Equal(expectedCount2, res.Count);
            Assert.All(res, item => Assert.InRange(item.ID, low: 1, high: expectedCount2));
        }

        [Fact]
        public void ThrowAlreadyExistsExcCreateBoardGameTest()
        {
            var boardGame = new BoardGame
            {
                Title = "Title1",
                Produser = "Producer1",
                Year = 2001,
            };

            System.Action action = () => _service.CreateBoardGame(boardGame);

            Assert.Throws<AlreadyExistsBoardGameException>(action);
        }

        [Fact]
        public void UpdateBoardGameTest()
        {
            var expectedCount = _mockBoardGames.Count;
            var boardGame = new BoardGame
            {
                ID = 1,
                Title = "Бункер",
                Produser = "Экономикус",
                Year = 2001,
                MaxAge = 99,
                MinAge = 18,
                MaxPlayerNum = 16,
                MinPlayerNum = 4,
                MaxDuration = 60,
                MinDuration = 30
            };

            var res = _service.GetBoardGames();
            Assert.Equal(expectedCount, res.Count);

            _service.UpdateBoardGame(boardGame);

            res = _service.GetBoardGames();

            Assert.Equal(expectedCount, res.Count);
            Assert.All(res, item => Assert.InRange(item.ID, low: 1, high: expectedCount));
            var newVal = res.Find(item => item.ID == boardGame.ID);
            Assert.Equal(newVal.ID, boardGame.ID);
            Assert.Equal(newVal.Title, boardGame.Title);
            Assert.Equal(newVal.Produser, boardGame.Produser);
            Assert.Equal(newVal.Year, boardGame.Year);
            Assert.Equal(newVal.MinAge, boardGame.MinAge);
            Assert.Equal(newVal.MaxAge, boardGame.MaxAge);
            Assert.Equal(newVal.MinPlayerNum, boardGame.MinPlayerNum);
            Assert.Equal(newVal.MaxPlayerNum, boardGame.MaxPlayerNum);
            Assert.Equal(newVal.MinDuration, boardGame.MinDuration);
            Assert.Equal(newVal.MaxDuration, boardGame.MaxDuration);
        }

        [Fact]
        public void ThrowNotExistsExcUpdateBoardGameTest()
        {
            var boardGame = new BoardGame { ID = 100 };

            System.Action action = () => _service.UpdateBoardGame(boardGame);

            Assert.Throws<NotExistsBoardGameException>(action);
        }

        [Fact]
        public void DeleteBoardGameTest()
        {
            var expectedCount = _mockBoardGames.Count;
            var boardGame = new BoardGame { ID = 1 };

            var res = _service.GetBoardGames();
            Assert.Equal(expectedCount, res.Count);

            _service.DeleteBoardGame(boardGame);

            res = _service.GetBoardGames();

            Assert.Equal(expectedCount - 1, res.Count);
            Assert.Null(_mockBoardGames.Find(x => x.ID == boardGame.ID));
        }

        [Fact]
        public void ThrowNotExistsExcDeleteBoardGameTest()
        {
            var boardGame = new BoardGame { ID = 100 };

            System.Action action = () => _service.DeleteBoardGame(boardGame);

            Assert.Throws<NotExistsBoardGameException>(action);
        }

        [Fact]
        public void GetEventsByGameTest()
        {
            var expectedCount = 1;
            var game = new BoardGame { ID = 1 };

            var events = _service.GetEventsByGame(game);

            Assert.Equal(expectedCount, events.Count);
            Assert.Equal("First", events.First().Title);
        }
    }
}
