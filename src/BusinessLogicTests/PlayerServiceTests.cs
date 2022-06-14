using Xunit;
using Moq;
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
        private readonly List<BGERegistration> _mockRegistrations;

        public PlayerServiceTests()
        {
            _mockPlayers = new List<Player>
            {
                new Player
                {
                    ID = 1,
                    Name = "MyMiDi",
                    League = "Новичок",
                    Rating = 100
                },
                new Player
                {
                    ID = 2,
                    Name = "amunra2",
                    League = "Просто зашел",
                    Rating = 0
                },
                new Player
                {
                    ID = 3,
                    Name = "hamzreg",
                    League = "Бывалый",
                    Rating = 1000
                },
                new Player
                {
                    ID = 4,
                    Name = "Мак",
                    League = "Новичок",
                    Rating = 0
                }
            };
            _mockBGEvents = new List<BoardGameEvent>
            {
                new BoardGameEvent { ID = 1, Title = "First" }
            };
            _mockRegistrations = new List<BGERegistration>()
            {
                new BGERegistration { ID = 1, PlayerID = 2, BoardGameEventID = 1}
            };

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

            mockRepo.Setup(repo => repo.AddToEvent(It.IsAny<BGERegistration>())).Callback(
                (BGERegistration registration) =>
                {
                    registration.ID = _mockPlayers.Count + 1;
                    _mockRegistrations.Add(registration);
                }
                );
            mockRepo.Setup(repo => repo.DeleteFromEvent(It.IsAny<BGERegistration>())).Callback(
                (BGERegistration registration) =>
                _mockRegistrations.RemoveAll(x => x.PlayerID == registration.PlayerID
                                       && x.BoardGameEventID == registration.BoardGameEventID));
            mockRepo.Setup(repo =>
            repo.GetRegistrationID(It.IsAny<BGERegistration>())).Returns(
                (BGERegistration registration) =>
                {
                    var foundReg = _mockRegistrations.Find(x =>
                        x.BoardGameEventID == registration.BoardGameEventID
                        && x.PlayerID == registration.PlayerID);
                    return (foundReg == null) ? -1 : foundReg.ID;
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

            //mockRepo.Setup(repo => repo.GetByEvent(It.IsAny<long>())).Returns(
            //    (long eventID) =>
            //    {
            //        var playerIDs = _mockRegistrations
            //                        .FindAll(x => x.BoardGameEventID == eventID)
            //                        .Select(x => x.PlayerID);
            //        return _mockPlayers.FindAll(x => playerIDs.Contains(x.ID));
            //    });

            _mockRepo = mockRepo.Object;

            var mockUserRepo = new Mock<IUserRepository>();
            var userService = new UserService(mockUserRepo.Object);
            userService.SetCurrentUser(new User()
            {
                Name = "test",
                Role = "player",
                RoleID = 1
            });

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
            var player = new Player
            {
                ID = 1,
                Name = "MyMiDi",
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
            var player = new Player { ID = 100 };

            void action() => _service.UpdatePlayer(player);

            Assert.Throws<NotExistsPlayerException>(action);
        }

        [Fact]
        public void DeletePlayerTest()
        {
            var expectedCount = _mockPlayers.Count;
            var player = new Player { ID = 1 };

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
            var player = new Player { ID = 100 };

            void action() => _service.DeletePlayer(player);

            Assert.Throws<NotExistsPlayerException>(action);
        }

        [Fact]
        public void RegisterPlayerForEventTest()
        {
            var expectedCount = _mockRegistrations.Count + 1;

            var bgEvent = new BoardGameEvent() { ID = 1 };

            _service.RegisterCurrentPlayerForEvent(bgEvent);

            Assert.Equal(expectedCount, _mockRegistrations.Count);
            Assert.NotNull(_mockRegistrations.Find(x => x.PlayerID == 1
                                                     && x.BoardGameEventID == bgEvent.ID));
        }

        [Fact]
        public void UnregisterPlayerForEventTest()
        {
            _mockRegistrations.Add(new BGERegistration
            {
                ID = 2,
                BoardGameEventID = 1,
                PlayerID = 1
            });
            var expectedCount = _mockRegistrations.Count - 1;

            var bgEvent = new BoardGameEvent() { ID = 1 };

            _service.UnregisterCurrentPlayerForEvent(bgEvent);

            Assert.Equal(expectedCount, _mockRegistrations.Count);
        }

        [Fact]
        public void GetPlayerEventsTest()
        {
            _mockRegistrations.Add(new BGERegistration
            {
                ID = 2,
                BoardGameEventID = 1,
                PlayerID = 1
            });
            var expectedCount = 1;

            var events = _service.GetCurrentPlayerEvents();

            Assert.Equal(expectedCount, events.Count);
            Assert.Equal("First", events.First().Title);
        }
    }
}
