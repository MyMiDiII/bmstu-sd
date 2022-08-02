using Xunit;
using Moq;
using System.Linq;
using System.Collections.Generic;
using System;

using BusinessLogic.IRepositories;
using BusinessLogic.Models;
using BusinessLogic.Services;
using BusinessLogic.Exceptions;

namespace BusinessLogicTests
{
    public class BoardGameEventServiceTests
    {
        private readonly IBoardGameEventRepository _mockRepo;
        private readonly IBoardGameEventService _service;

        private readonly List<BoardGameEvent> _mockBoardGameEvents;
        private readonly List<BoardGame> _mockBoardGames;
        private readonly List<EventGame> _mockEventGames;
        private readonly List<Player> _mockPlayers;
        private readonly List<PlayerRegistration> _mockRegistrations;

        public BoardGameEventServiceTests()
        {
            _mockBoardGameEvents = new List<BoardGameEvent>
            {
                new BoardGameEvent("Title1", new DateOnly(2022, 6, 1))
                {
                    ID = 1,
                    StartTime = new TimeOnly(13, 0),
                    Duration = 300,
                    Cost = 0,
                    Purchase = true,
                    OrganizerID = 1,
                    VenueID = 1
                },
                new BoardGameEvent("Title2", new DateOnly(2022, 6, 13))
                {
                    ID = 2,
                    StartTime = new TimeOnly(13, 0),
                    Duration = 300,
                    Cost = 400,
                    Purchase = false,
                    OrganizerID = 2,
                    VenueID = 1
                },
                new BoardGameEvent("Title3", new DateOnly(2022, 6, 17))
                {
                    ID = 3,
                    StartTime = new TimeOnly(13, 0),
                    Duration = 300,
                    Cost = 250,
                    Purchase = false,
                    OrganizerID = 1,
                    VenueID = 2
                }
            };
            _mockBoardGames = new List<BoardGame>
            {
                new BoardGame("Title1")
                {
                    ID = 1,
                    Produser = "Producer1",
                    Year = 2001,
                }
            };
            _mockEventGames = new List<EventGame> { new EventGame(1, 1) };
            _mockPlayers = new List<Player>
            {
                new Player("MyMiDi")
                {
                    ID = 1,
                    League = "Новичок",
                    Rating = 100
                }
            };
            _mockRegistrations = new List<PlayerRegistration>() { new PlayerRegistration(1, 1) };

            var mockRepo = new Mock<IBoardGameEventRepository>();
            mockRepo.Setup(repo => repo.GetAll()).Returns(_mockBoardGameEvents);
            mockRepo.Setup(repo => repo.Add(It.IsAny<BoardGameEvent>())).Callback(
                (BoardGameEvent boardGameEvent) =>
                {
                    boardGameEvent.ID = _mockBoardGameEvents.Count + 1;
                    _mockBoardGameEvents.Add(boardGameEvent);
                }
                );
            mockRepo.Setup(repo => repo.GetByID(It.IsAny<long>())).Returns(
                (long id) => _mockBoardGameEvents.Find(x => x.ID == id));
            mockRepo.Setup(repo => repo.Update(It.IsAny<BoardGameEvent>())).Callback(
                (BoardGameEvent boardGameEvent) =>
                {
                    _mockBoardGameEvents
                        .FindAll(x => x.ID == boardGameEvent.ID)
                        .ForEach(x =>
                        {
                            x.Title = boardGameEvent.Title;
                            x.Date = boardGameEvent.Date;
                            x.StartTime = boardGameEvent.StartTime;
                            x.Duration = boardGameEvent.Duration;
                            x.Cost = boardGameEvent.Cost;
                            x.Purchase = boardGameEvent.Purchase;
                            x.OrganizerID = boardGameEvent.OrganizerID;
                            x.VenueID = boardGameEvent.VenueID;
                        });
                }
                );
            mockRepo.Setup(repo => repo.Delete(It.IsAny<BoardGameEvent>())).Callback(
                (BoardGameEvent boardGameEvent) => _mockBoardGameEvents.RemoveAll(x => x.ID == boardGameEvent.ID));
            mockRepo.Setup(repo => repo.GetEventGames(It.IsAny<long>())).Returns(
                (long eventID) =>
                {
                    var gamesID = _mockEventGames
                                    .FindAll(x => x.BoardGameEventID == eventID)
                                    .Select(x => x.BoardGameID);
                    return _mockBoardGames.FindAll(x => gamesID.Contains(x.ID));
                }
                );
            mockRepo.Setup(repo => repo.GetEventPlayers(It.IsAny<long>())).Returns(
                (long eventID) =>
                {
                    var playerIDs = _mockRegistrations
                                    .FindAll(x => x.BoardGameEventID == eventID)
                                    .Select(x => x.PlayerID);
                    return _mockPlayers.FindAll(x => playerIDs.Contains(x.ID));
                });

            _mockRepo = mockRepo.Object;
            _service = new BoardGameEventService(_mockRepo);
        }

        [Fact]
        public void GetBoardGameEventsTest()
        {
            var expectedCount = _mockBoardGameEvents.Count;

            var res = _service.GetBoardGameEvents();

            Assert.IsType<List<BoardGameEvent>>(res);
            Assert.Equal(expectedCount, res.Count);
            Assert.All(res, item => Assert.InRange(item.ID, low: 1, high: expectedCount));
        }

        [Fact]
        public void CreateBoardGameEventTest()
        {
            var expectedCount = _mockBoardGameEvents.Count;
            var expectedCount2 = expectedCount + 1;
            var res = _service.GetBoardGameEvents();
            var boardGameEvent = new BoardGameEvent("Title4", new DateOnly(2022, 6, 17))
            {
                StartTime = new TimeOnly(13, 0),
                Duration = 300,
                Cost = 250,
                Purchase = false,
                OrganizerID = 3,
                VenueID = 4
            };

            Assert.Equal(expectedCount, res.Count);

            _service.CreateBoardGameEvent(boardGameEvent);

            res = _service.GetBoardGameEvents();

            Assert.Equal(expectedCount2, res.Count);
            Assert.All(res, item => Assert.InRange(item.ID, low: 1, high: expectedCount2));
        }

        [Fact]
        public void ThrowAlreadyExistsExcCreateBoardGameEventTest()
        {
            var boardGameEvent = new BoardGameEvent("Title2", new DateOnly(2022, 6, 13))
            {
                StartTime = new TimeOnly(15, 0),
                Duration = 300,
                Cost = 400,
                Purchase = false,
                OrganizerID = 2,
                VenueID = 1
            };

            void action() => _service.CreateBoardGameEvent(boardGameEvent);

            Assert.Throws<AlreadyExistsBoardGameEventException>(action);
        }

        [Fact]
        public void UpdateBoardGameEventTest()
        {
            var expectedCount = _mockBoardGameEvents.Count;
            var boardGameEvent = new BoardGameEvent("Title2", new DateOnly(2022, 6, 2))
            {
                ID = 1,
                StartTime = new TimeOnly(13, 0),
                Duration = 300,
                Cost = 0,
                Purchase = true,
                OrganizerID = 1,
                VenueID = 1
            };

            var res = _service.GetBoardGameEvents();
            Assert.Equal(expectedCount, res.Count);

            _service.UpdateBoardGameEvent(boardGameEvent);

            res = _service.GetBoardGameEvents();

            Assert.Equal(expectedCount, res.Count);
            Assert.All(res, item => Assert.InRange(item.ID, low: 1, high: expectedCount));
            var newVal = res.Find(item => item.ID == boardGameEvent.ID);
            Assert.Equal(newVal?.ID, boardGameEvent.ID);
            Assert.Equal(newVal?.Title, boardGameEvent.Title);
            Assert.Equal(newVal?.Date, boardGameEvent.Date);
            Assert.Equal(newVal?.StartTime, boardGameEvent.StartTime);
            Assert.Equal(newVal?.Duration, boardGameEvent.Duration);
            Assert.Equal(newVal?.Cost, boardGameEvent.Cost);
            Assert.Equal(newVal?.Purchase, boardGameEvent.Purchase);
            Assert.Equal(newVal?.OrganizerID, boardGameEvent.OrganizerID);
            Assert.Equal(newVal?.VenueID, boardGameEvent.VenueID);
        }

        [Fact]
        public void ThrowNotExistsExcUpdateBoardGameEventTest()
        {
            var boardGameEvent = new BoardGameEvent("title", new DateOnly(2001, 1, 1)) { ID = 100 };

            void action() => _service.UpdateBoardGameEvent(boardGameEvent);

            Assert.Throws<NotExistsBoardGameEventException>(action);
        }

        [Fact]
        public void DeleteBoardGameEventTest()
        {
            var expectedCount = _mockBoardGameEvents.Count;
            var boardGameEvent = new BoardGameEvent("title", new DateOnly(2001, 1, 1)) { ID = 1 };

            var res = _service.GetBoardGameEvents();
            Assert.Equal(expectedCount, res.Count);

            _service.DeleteBoardGameEvent(boardGameEvent);

            res = _service.GetBoardGameEvents();

            Assert.Equal(expectedCount - 1, res.Count);
            Assert.Null(_mockBoardGameEvents.Find(x => x.ID == boardGameEvent.ID));
        }

        [Fact]
        public void ThrowNotExistsExcDeleteBoardGameEventTest()
        {
            var boardGameEvent = new BoardGameEvent("title", new DateOnly(2001, 1, 1)) { ID = 100 };

            void action() => _service.DeleteBoardGameEvent(boardGameEvent);

            Assert.Throws<NotExistsBoardGameEventException>(action);
        }

        [Fact]
        public void GetGamesByEventTest()
        {
            var expectedCount = 1;
            var bgEvent = new BoardGameEvent("title", new DateOnly(2001, 1, 1)) { ID = 1 };

            var games = _service.GetGamesByEvent(bgEvent);

            Assert.Equal(expectedCount, games.Count);
            Assert.Equal("Title1", games.First().Title);
        }

        [Fact]
        public void GetPlayersByEventTest()
        {
            var expectedCount = 1;
            var bgEvent = new BoardGameEvent("title", new DateOnly(2001, 1, 1)) { ID = 1 };

            var players = _service.GetPlayersByEvent(bgEvent);

            Assert.Equal(expectedCount, players.Count);
            Assert.Equal("MyMiDi", players.First().Name);
        }
    }
}
