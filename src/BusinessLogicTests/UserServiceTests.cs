using Xunit;
using Moq;
using System.Collections.Generic;
using System.Linq;

using BusinessLogic.Config;
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
        private readonly IEncryptionService _encryptionService;
        private readonly List<Player> _mockPlayers;

        public UserServiceTests()
        {
            _encryptionService = new BCryptEntryptionService();

            _mockUsers = new List<User>
            {
                new User("MyMiDi", _encryptionService.HashPassword("strong"))
                {
                    ID = 1,
                    Roles = new List<Role> { new Role("admin") }
                },
                new User("amunra2", _encryptionService.HashPassword("123simple123"))
                {
                    ID = 2,
                    Roles = new List<Role> { new Role("organizer") }
                },
                new User("hamzreg", _encryptionService.HashPassword("hoba"))
                {
                    ID = 3,
                    Roles = new List<Role> { new Role("player") }
                },
                new User("guest", _encryptionService.HashPassword("guest"))
                {
                    ID = 4,
                    Roles = new List<Role> { new Role("guest") }
                }
            };
            _mockPlayers = new List<Player>();

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
                            x.Roles= user.Roles;
                        });
                }
                );
            mockRepo.Setup(repo => repo.Delete(It.IsAny<User>())).Callback(
                (User user) => _mockUsers.RemoveAll(x => x.ID == user.ID));
            mockRepo.Setup(repo => repo.GetUserRoles(It.IsAny<long>())).Returns(
                (long id) =>
                {
                    var found = _mockUsers.Find(x => x.ID == id)?.Roles;
                    return found ?? new List<Role>();
                });
            mockRepo.Setup(repo => repo.AddWithBasicRole(It.IsAny<User>())).Callback(
                (User user) =>
                {
                    var player = new Player(user.Name)
                    {
                        ID = _mockPlayers.Count + 1,
                        League = PlayerConfig.Leagues.First(),
                        Rating = 0
                    };
                    _mockPlayers.Add(player);

                    var newUser = new User(user.Name, user.Password)
                    {
                        ID = _mockUsers.Count + 1,
                        Roles = new List<Role>()
                        {
                            new Role("player")
                            {
                                RoleID = player.ID
                            }
                        }
                    };

                    _mockUsers.Add(newUser);
                });

            _mockRepo = mockRepo.Object;
            _service = new UserService(_mockRepo, _encryptionService);
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
            var user = new User("new", "123")
            {
                Roles = new List<Role> { new Role("player") }
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
            var user = new User("amunra2", "123")
            {
                Roles = new List<Role> { new Role("organizer") }
            };

            void action() => _service.CreateUser(user);

            Assert.Throws<AlreadyExistsUserException>(action);
        }

        [Fact]
        public void UpdateUserTest()
        {
            var expectedCount = _mockUsers.Count;
            var user = new User("MyMiDiAdmin", "admin")
            {
                ID = 1,
                Roles = new List<Role> { new Role("admin") }
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
            Assert.Equal(newVal?.Roles.Count, user.Roles.Count);
            Assert.Equal(newVal?.Roles[0].RoleName, user.Roles[0].RoleName);
        }

        [Fact]
        public void ThrowNotExistsExcUpdateUserTest()
        {
            var user = new User("name", "pass") { ID = 100 };

            void action() => _service.UpdateUser(user);

            Assert.Throws<NotExistsUserException>(action);
        }

        [Fact]
        public void DeleteUserTest()
        {
            var expectedCount = _mockUsers.Count;
            var user = new User("name", "pass") { ID = 2 };

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
            var user = new User("name", "pass") { ID = 100 };

            void action() => _service.DeleteUser(user);

            Assert.Throws<NotExistsUserException>(action);
        }

        [Fact]
        public void ThrowNotExistsExcLoginTest()
        {
            var request = new LoginRequest("NotUser", "123");

            void action() => _service.Login(request);

            Assert.Throws<NotExistsUserException>(action);
        }

        [Fact]
        public void ThrowIncorrectUserPasswordExcLoginTest()
        {
            var request = new LoginRequest("MyMiDi", "verystrong" );

            void action() => _service.Login(request);

            Assert.Throws<IncorrectUserPasswordException>(action);
        }

        [Fact]
        public void SuccessfulLoginTest()
        {
            var request = new LoginRequest("MyMiDi", "strong");

            _service.Login(request);

            var curUser = _service.GetCurrentUser();

            Assert.Equal(1, curUser.ID);
            Assert.Equal("MyMiDi", curUser.Name);
            Assert.Equal(curUser.Roles.Count, _mockUsers[0].Roles.Count);
            Assert.Equal(curUser.Roles[0].RoleName, _mockUsers[0].Roles[0].RoleName);
        }

        [Fact]
        public void ThrowAlreadyExistsExcRegisterTest()
        {
            var request = new RegisterRequest("MyMiDi", "123");

            void action() => _service.Register(request);

            Assert.Throws<AlreadyExistsUserException>(action);
        }

        [Fact]
        public void SuccessfulRegisterTest()
        {
            var request = new RegisterRequest("NewUser", "password");

            _service.Register(request);

            var newUser = _mockUsers.Find(x => x.Name == request.Name);
            Assert.NotNull(newUser);
            Assert.Equal("NewUser", newUser?.Name);
            Assert.True(_encryptionService.ValidatePassword(request.Password, newUser?.Password ?? "wrong"));
            Assert.Equal("player", newUser?.Roles[0].RoleName);
            Assert.Single(_mockPlayers);
        }
    }
}
