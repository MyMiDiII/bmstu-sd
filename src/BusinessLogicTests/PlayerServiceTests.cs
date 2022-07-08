using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

using BusinessLogic.IRepositories;
using BusinessLogic.Models;
using BusinessLogic.Services;
using BusinessLogic.Exceptions;
using BusinessLogic.Config;

namespace BusinessLogicTests
{
    public class PlayerServiceTests
    {
        private readonly IPlayerRepository _mockRepo;
        private readonly IPlayerService _service;

        private readonly List<Player> _mockPlayers;
        private readonly List<BoardGameEvent> _mockBGEvents;
        private readonly List<BoardGame> _mockBoardGames;
        private readonly List<PlayerRegistration> _mockRegistrations;
        private readonly List<FavoriteBoardGame> _mockFavoriteGames;

        public PlayerServiceTests()
        {
            _mockPlayers = new List<Player>
            {
                new Player("MyMiDi")
                {
                    ID = 1,
                    League = "Новичок",
                    Rating = 100
                },
                new Player("amunra2")
                {
                    ID = 2,
                    League = "Просто зашел",
                    Rating = 0
                },
                new Player("hamzreg")
                {
                    ID = 3,
                    League = "Бывалый",
                    Rating = 1000
                },
                new Player("Мак")
                {
                    ID = 4,
                    League = "Новичок",
                    Rating = 0
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
            _mockBGEvents = new List<BoardGameEvent>
            {
                new BoardGameEvent("First", new DateOnly(2001, 1, 1)) { ID = 1 }
            };
            _mockRegistrations = new List<PlayerRegistration>() { new PlayerRegistration(2, 1) };
            _mockFavoriteGames = new List<FavoriteBoardGame>() { new FavoriteBoardGame(2, 1) };

            var mockRepo = new Mock<IPlayerRepository>();

            mockRepo.Setup(repo => repo.GetAll()).Returns(_mockPlayers);
            mockRepo.Setup(repo => repo.GetByID(It.IsAny<long>())).Returns(
                (long id) => _mockPlayers.Find(x => x.ID == id));
            mockRepo.Setup(repo => repo.GetByName(It.IsAny<string>())).Returns(
                (string name) => _mockPlayers.Find(x => x.Name == name));

            mockRepo.Setup(repo => repo.Add(It.IsAny<Player>())).Callback(
                (Player player) =>
                {
                    player.ID = _mockPlayers.Count + 1;
                    _mockPlayers.Add(player);
                }
                );
            mockRepo.Setup(repo => repo.Update(It.IsAny<Player>())).Callback(
                (Player player) =>
                {
                    _mockPlayers
                        .FindAll(x => x.ID == player.ID)
                        .ForEach(x =>
                        {
                            x.Name = player.Name;
                            x.League = player.League;
                            x.Rating = player.Rating;
                        });
                }
                );
            mockRepo.Setup(repo => repo.Delete(It.IsAny<Player>())).Callback(
                (Player player) => _mockPlayers.RemoveAll(x => x.ID == player.ID));

            mockRepo.Setup(repo => repo.AddToEvent(It.IsAny<long>(), It.IsAny<long>())).Callback(
                (long eventID, long playerID) =>
                {
                    var registration = new PlayerRegistration(eventID, playerID);
                    _mockRegistrations.Add(registration);
                }
                );
            mockRepo.Setup(repo => repo.DeleteFromEvent(It.IsAny<long>(), It.IsAny<long>())).Callback(
                (long eventID, long playerID) =>
                _mockRegistrations.RemoveAll(x => x.BoardGameEventID == eventID && x.PlayerID == playerID));
            mockRepo.Setup(repo =>
            repo.CheckPlayerRegistration(It.IsAny<long>(), It.IsAny<long>())).Returns(
                (long eventID, long playerID) =>
                {
                   return _mockRegistrations.Where(x => x.BoardGameEventID == eventID
                                                     && x.PlayerID == playerID).Any();
                });
            mockRepo.Setup(repo => repo.GetPlayerEvents(It.IsAny<long>())).Returns(
                (long playerID) =>
                {
                    var eventsIDs = _mockRegistrations
                                    .FindAll(x => x.PlayerID == playerID)
                                    .Select(x => x.BoardGameEventID);
                    return _mockBGEvents.FindAll(x => eventsIDs.Contains(x.ID));
                }
                );
            _mockRepo = mockRepo.Object;

            var mockUserRepo = new Mock<IUserRepository>();
            mockUserRepo.Setup(repo => repo.GetDefauldUser()).Returns(
                new User("test", "123") { Roles = new List<Role> { new Role("player") { RoleID = 1 } } });
            var userService = new UserService(mockUserRepo.Object, new BCryptEntryptionService());

            _service = new PlayerService(_mockRepo, userService);
        }

        [Fact]
        public void GetPlayersTest()
        {
            var expectedCount = _mockPlayers.Count;

            var res = _service.GetPlayers();

            Assert.IsType<List<Player>>(res);
            Assert.Equal(expectedCount, res.Count);
            Assert.All(res, item => Assert.InRange(item.ID, low: 1, high: expectedCount));
        }

        [Fact]
        public void CreatePlayerTest()
        {
            var expectedCount = _mockPlayers.Count;
            var expectedCount2 = expectedCount + 1;
            var res = _service.GetPlayers();

            var playerName = "new";

            Assert.Equal(expectedCount, res.Count);

            _service.CreatePlayer(playerName);

            res = _service.GetPlayers();

            Assert.Equal(expectedCount2, res.Count);
            Assert.All(res, item => Assert.InRange(item.ID, low: 1, high: expectedCount2));
            Assert.Equal(res.Last().League, PlayerConfig.Leagues.First());
            Assert.Equal(res.Last().Rating, (uint) 0);
        }

        [Fact]
        public void ThrowAlreadyExistsExcCreatePlayerTest()
        {
            var playerName = "MyMiDi";

            void action() => _service.CreatePlayer(playerName);

            Assert.Throws<AlreadyExistsPlayerException>(action);
        }

        [Fact]
        public void UpdatePlayerTest()
        {
            var expectedCount = _mockPlayers.Count;
            var player = new Player("MyMiDi")
            {
                ID = 1,
                League = "Бывалый",
                Rating = 0
            };

            var res = _service.GetPlayers();
            Assert.Equal(expectedCount, res.Count);

            _service.UpdatePlayer(player);

            res = _service.GetPlayers();

            Assert.Equal(expectedCount, res.Count);
            Assert.All(res, item => Assert.InRange(item.ID, low: 1, high: expectedCount));
            var newVal = res.Find(item => item.ID == player.ID);
            Assert.NotNull(newVal);
            Assert.Equal(newVal?.ID, player.ID);
            Assert.Equal(newVal?.Name, player.Name);
            Assert.Equal(newVal?.League, player.League);
            Assert.Equal(newVal?.Rating, player.Rating);
        }

        [Fact]
        public void ThrowNotExistsExcUpdatePlayerTest()
        {
            var player = new Player("MyMiDi") { ID = 100 };

            void action() => _service.UpdatePlayer(player);

            Assert.Throws<NotExistsPlayerException>(action);
        }

        [Fact]
        public void DeletePlayerTest()
        {
            var expectedCount = _mockPlayers.Count;
            var player = new Player("MyMiDi") { ID = 1 };

            var res = _service.GetPlayers();
            Assert.Equal(expectedCount, res.Count);

            _service.DeletePlayer(player);

            res = _service.GetPlayers();

            Assert.Equal(expectedCount - 1, res.Count);
            Assert.Null(_mockPlayers.Find(x => x.ID == player.ID));
        }

        [Fact]
        public void ThrowNotExistsExcDeletePlayerTest()
        {
            var player = new Player("MyMiDi") { ID = 100 };

            void action() => _service.DeletePlayer(player);

            Assert.Throws<NotExistsPlayerException>(action);
        }

        [Fact]
        public void RegisterPlayerForEventTest()
        {
            var expectedCount = _mockRegistrations.Count + 1;

            var bgEvent = new BoardGameEvent("First", new DateOnly(2001, 1, 1)) { ID = 1 };

            _service.RegisterCurrentPlayerForEvent(bgEvent);

            Assert.Equal(expectedCount, _mockRegistrations.Count);
            Assert.NotNull(_mockRegistrations.Find(x => x.PlayerID == 1
                                                     && x.BoardGameEventID == bgEvent.ID));
        }

        [Fact]
        public void UnregisterPlayerForEventTest()
        {
            _mockRegistrations.Add(new PlayerRegistration(1, 1));
            var expectedCount = _mockRegistrations.Count - 1;

            var bgEvent = new BoardGameEvent("First", new DateOnly(2001, 1, 1)) { ID = 1 };

            _service.UnregisterCurrentPlayerForEvent(bgEvent);

            Assert.Equal(expectedCount, _mockRegistrations.Count);
        }

        [Fact]
        public void GetPlayerEventsTest()
        {
            _mockRegistrations.Add(new PlayerRegistration(1, 1));
            var expectedCount = 1;

            var events = _service.GetCurrentPlayerEvents();

            Assert.Equal(expectedCount, events.Count);
            Assert.Equal("First", events.First().Title);
        }


        [Fact]
        public void GetPlayerFavoritesTest()
        {
            _mockFavoriteGames.Add(new FavoriteBoardGame(1, 1));
            var expectedCount = 1;

            var games = _service.GetCurrentPlayerFavorites();

            Assert.Equal(expectedCount, games.Count);
            Assert.Equal("Title1", games.First().Title);
        }
    }
}
