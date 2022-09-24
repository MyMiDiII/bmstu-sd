using System.Data.Common;

using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;

using DataAccess;
using DataAccess.Repositories;
using BusinessLogic.Models;

namespace DataAccessTests
{
    public class UserRepositoryTests
    {
        private readonly DbConnection _dbconnection;
        private readonly DbContextOptions<BGEContext> _dbContextOptions;

        public UserRepositoryTests()
        {
            _dbconnection = new SqliteConnection("Filename=:memory:");
            _dbconnection.Open();

            _dbContextOptions = new DbContextOptionsBuilder<BGEContext>()
                .UseSqlite(_dbconnection)
                .Options;

            using var context = new BGEContext(_dbContextOptions);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.Users.AddRange(new User("MyMiDi", "123"), new User("???", "123"));
            context.SaveChanges();
            context.Roles.AddRange(new Role("organizer") { UserID = 2 }, new Role("player") { UserID = 3 });
            context.SaveChanges();
        }

        private BGEContext CreateContext() => new(_dbContextOptions);
        private UserRepository CreateUserRepository() => new(CreateContext());
        private static UserRepository CreateUserRepository(BGEContext context) =>
            new(context);

        [Fact]
        public void UserGetAllTest()
        {
            var rep = CreateUserRepository();
            var users = rep.GetAll();

            Assert.Equal(3, users.Count);
            Assert.Collection(
                users,
                u =>
                {
                    Assert.Equal(1, u.ID);
                    Assert.Equal("guest", u.Name);
                    Assert.Equal("guest", u.Password);
                    Assert.Single(u.Roles);
                },
                u =>
                {
                    Assert.Equal(2, u.ID);
                    Assert.Equal("MyMiDi", u.Name);
                    Assert.Equal("123", u.Password);
                },
                u =>
                {
                    Assert.Equal(3, u.ID);
                    Assert.Equal("???", u.Name);
                    Assert.Equal("123", u.Password);
                }
            );
        }

        [Fact]
        public void UserGetByIDTest()
        {
            var rep = CreateUserRepository();
            var user = rep.GetByID(1);

            Assert.NotNull(user);
            Assert.Equal(1, user?.ID);
            Assert.Equal("guest", user?.Name);
            Assert.Equal("guest", user?.Password);
            Assert.Collection(user?.Roles, r => { Assert.Equal("guest", r.RoleName); } );
        }

        [Fact]
        public void UserAddTest()
        {
            var context = CreateContext();
            var rep = CreateUserRepository(context);
            var user = new User("111", "111");

            rep.Add(user);

            Assert.Equal(4, context.Users.Count());
            var added = context.Users.Single(p => p.Name == "111");
            Assert.NotNull(added);
            Assert.Equal(4, added?.ID);
            Assert.Equal("111", added?.Name);
            Assert.Equal("111", added?.Password);
        }

        [Fact]
        public void UserUpdateTest()
        {
            var context = CreateContext();
            var rep = CreateUserRepository(context);
            var user = new User("Three?", "???") { ID = 3 };

            rep.Update(user);

            Assert.Equal(3, context.Users.Count());
            Assert.Null(context.Users.FirstOrDefault(p => p.Name == "???"));
            var updated = context.Users.Single(p => p.Name == "Three?");
            Assert.NotNull(updated);
            Assert.Equal(3, updated?.ID);
            Assert.Equal("Three?", updated?.Name);
            Assert.Equal("???", updated?.Password);
        }

        [Fact]
        public void UserDeleteTest()
        {
            var context = CreateContext();
            var rep = CreateUserRepository(context);
            var user = new User("???", "123") { ID = 3 };

            rep.Delete(user);

            Assert.Equal(2, context.Users.Count());
            var deleted = context.Users.SingleOrDefault(g => g.Name == "???");
            Assert.Null(deleted);
        }

        [Fact]
        public void UserGetByNameTest()
        {
            var rep = CreateUserRepository();

            var user = rep.GetByName("???");

            Assert.NotNull(user);
            Assert.Equal(3, user?.ID);
            Assert.Equal("???", user?.Name);
            Assert.Equal("123", user?.Password);
        }

        [Fact]
        public void UserGetByRoleTest()
        {
            var rep = CreateUserRepository();

            var users = rep.GetByRole("player");

            Assert.Single(users);
            Assert.Collection(
                users,
                p =>
                {
                    Assert.Equal(3, p.ID);
                    Assert.Equal("???", p.Name);
                    Assert.Equal("123", p.Password);
                }
            );
        }

        [Fact]
        public void UserGetRolesTest()
        {
            var rep = CreateUserRepository();

            var roles = rep.GetUserRoles(2);

            Assert.Single(roles);
            Assert.Collection(
                roles,
                r =>
                {
                    Assert.Equal("organizer", r.RoleName);
                    Assert.Equal(2, r.UserID);
                }
            );
        }

        [Fact]
        public void UserAddWithBasicRoleTest()
        {
            var context = CreateContext();
            var rep = CreateUserRepository(context);
            var user = new User("111", "111");

            rep.AddWithBasicRole(user);

            Assert.Equal(4, context.Users.Count());
            var added = context.Users.Single(p => p.Name == "111");
            Assert.NotNull(added);
            Assert.Equal(4, added?.ID);
            Assert.Equal("111", added?.Name);
            Assert.Equal("111", added?.Password);

            Assert.Equal(4, context.Roles.Count());
            Assert.Equal("player", context.Roles.Single(r => r.UserID == 4).RoleName);
        }
    }
}
