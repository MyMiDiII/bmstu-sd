using Xunit;
using Moq;
using System.Collections.Generic;

using BusinessLogic.IRepositories;
using BusinessLogic.Models;
using BusinessLogic.Services;
using BusinessLogic.Exceptions;

namespace BusinessLogicTests
{
    public class PlayerServiceTests
    {
        private IPlayerRepository _mockRepo;
        List<Player> _mockPlayers;
        private IPlayerService _service;

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

            var mockRepo = new Mock<IPlayerRepository>();
            mockRepo.Setup(repo => repo.GetAll()).Returns(_mockPlayers);
            mockRepo.Setup(repo => repo.Add(It.IsAny<Player>())).Callback(
                (Player player) =>
                {
                    player.ID = _mockPlayers.Count + 1;
                    _mockPlayers.Add(player);
                }
                );
            mockRepo.Setup(repo => repo.GetByID(It.IsAny<long>())).Returns(
                (long id) => _mockPlayers.Find(x => x.ID == id));
            mockRepo.Setup(repo => repo.GetByName(It.IsAny<string>())).Returns(
                (string name) => _mockPlayers.Find(x => x.Name == name));
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

            _mockRepo = mockRepo.Object;
            _service = new PlayerService(_mockRepo);
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
            var player = new Player
            {
                Name = "new",
                Rating = 20,
                League = "Легенда"
            };

            Assert.Equal(expectedCount, res.Count);

            _service.CreatePlayer(player);

            res = _service.GetPlayers();

            Assert.Equal(expectedCount2, res.Count);
            Assert.All(res, item => Assert.InRange(item.ID, low: 1, high: expectedCount2));
        }

        [Fact]
        public void ThrowAlreadyExistsExcCreatePlayerTest()
        {
            var player = new Player
            {
                Name = "MyMiDi",
                Rating = 0,
                League = "Просто зашел"
            };

            System.Action action = () => _service.CreatePlayer(player);

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
            Assert.Equal(newVal.ID, player.ID);
            Assert.Equal(newVal.Name, player.Name);
            Assert.Equal(newVal.League, player.League);
            Assert.Equal(newVal.Rating, player.Rating);
        }

        [Fact]
        public void ThrowNotExistsExcUpdatePlayerTest()
        {
            var player = new Player { ID = 100 };

            System.Action action = () => _service.UpdatePlayer(player);

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

            System.Action action = () => _service.DeletePlayer(player);

            Assert.Throws<NotExistsPlayerException>(action);
        }
    }
}
