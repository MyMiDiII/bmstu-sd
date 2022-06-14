using Xunit;
using Moq;
using System.Collections.Generic;

using BusinessLogic.IRepositories;
using BusinessLogic.Models;
using BusinessLogic.Services;
using BusinessLogic.Exceptions;

namespace BusinessLogicTests
{
    public class UserServiceTests
    {
        private readonly IUserRepository _mockRepo;
        private readonly List<User> _mockUsers;
        private readonly IUserService _service;

        public UserServiceTests()
        {
            _mockUsers = new List<User>
            {
                new User
                {
                    ID = 1,
                    Name = "MyMiDi",
                    Role = "admin"
                },
                new User
                {
                    ID = 2,
                    Name = "amunra2",
                    Role = "organizer"
                },
                new User
                {
                    ID = 3,
                    Name = "hamzreg",
                    Role = "player"
                },
                new User
                {
                    ID = 4,
                    Name = "guest",
                    Role = "guest"
                }
            };

            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(repo => repo.GetAll()).Returns(_mockUsers);
            mockRepo.Setup(repo => repo.Add(It.IsAny<User>())).Callback(
                (User user) =>
                {
                    user.ID = _mockUsers.Count + 1;
                    _mockUsers.Add(user);
                }
                );
            mockRepo.Setup(repo => repo.GetByID(It.IsAny<long>())).Returns(
                (long id) => _mockUsers.Find(x => x.ID == id));
            mockRepo.Setup(repo => repo.GetByName(It.IsAny<string>())).Returns(
                (string name) => _mockUsers.Find(x => x.Name == name));
            mockRepo.Setup(repo => repo.Update(It.IsAny<User>())).Callback(
                (User user) =>
                {
                    _mockUsers
                        .FindAll(x => x.ID == user.ID)
                        .ForEach(x =>
                        {
                            x.Name = user.Name;
                            x.Role = user.Role;
                        });
                }
                );
            mockRepo.Setup(repo => repo.Delete(It.IsAny<User>())).Callback(
                (User user) => _mockUsers.RemoveAll(x => x.ID == user.ID));

            _mockRepo = mockRepo.Object;
            _service = new UserService(_mockRepo);
        }

        [Fact]
        public void GetUsersTest()
        {
            var expectedCount = _mockUsers.Count;

            var res = _service.GetUsers();

            Assert.IsType<List<User>>(res);
            Assert.Equal(expectedCount, res.Count);
            Assert.All(res, item => Assert.InRange(item.ID, low: 1, high: expectedCount));
        }

        [Fact]
        public void CreateUserTest()
        {
            var expectedCount = _mockUsers.Count;
            var expectedCount2 = expectedCount + 1;
            var res = _service.GetUsers();
            var user = new User
            {
                Name = "new",
                Role = "player"
            };

            Assert.Equal(expectedCount, res.Count);

            _service.CreateUser(user);

            res = _service.GetUsers();

            Assert.Equal(expectedCount2, res.Count);
            Assert.All(res, item => Assert.InRange(item.ID, low: 1, high: expectedCount2));
        }

        [Fact]
        public void SameNameAnotherRoleCreateUserTest()
        {
            var expectedCount = _mockUsers.Count;
            var expectedCount2 = expectedCount + 1;
            var res = _service.GetUsers();
            var user = new User
            {
                Name = "amunra2",
                Role = "player"
            };

            Assert.Equal(expectedCount, res.Count);

            _service.CreateUser(user);

            res = _service.GetUsers();

            Assert.Equal(expectedCount2, res.Count);
            Assert.All(res, item => Assert.InRange(item.ID, low: 1, high: expectedCount2));
        }

        [Fact]
        public void ThrowAlreadyExistsExcCreateUserTest()
        {
            var user = new User
            {
                Name = "amunra2",
                Role = "organizer"
            };

            void action() => _service.CreateUser(user);

            Assert.Throws<AlreadyExistsUserException>(action);
        }

        [Fact]
        public void UpdateUserTest()
        {
            var expectedCount = _mockUsers.Count;
            var user = new User
            {
                ID = 1,
                Name = "MyMiDiAdmin",
                Role = "admin"
            };

            var res = _service.GetUsers();
            Assert.Equal(expectedCount, res.Count);

            _service.UpdateUser(user);

            res = _service.GetUsers();

            Assert.Equal(expectedCount, res.Count);
            Assert.All(res, item => Assert.InRange(item.ID, low: 1, high: expectedCount));
            var newVal = res.Find(item => item.ID == user.ID);
            Assert.Equal(newVal?.ID, user.ID);
            Assert.Equal(newVal?.Name, user.Name);
            Assert.Equal(newVal?.Role, user.Role);
        }

        [Fact]
        public void ThrowNotExistsExcUpdateUserTest()
        {
            var user = new User { ID = 100 };

            void action() => _service.UpdateUser(user);

            Assert.Throws<NotExistsUserException>(action);
        }

        [Fact]
        public void DeleteUserTest()
        {
            var expectedCount = _mockUsers.Count;
            var user = new User { ID = 2 };

            var res = _service.GetUsers();
            Assert.Equal(expectedCount, res.Count);

            _service.DeleteUser(user);

            res = _service.GetUsers();

            Assert.Equal(expectedCount - 1, res.Count);
            Assert.Null(_mockUsers.Find(x => x.ID == user.ID));
        }

        [Fact]
        public void ThrowNotExistsExcDeleteUserTest()
        {
            var user = new User { ID = 100 };

            void action() => _service.DeleteUser(user);

            Assert.Throws<NotExistsUserException>(action);
        }
    }
}
