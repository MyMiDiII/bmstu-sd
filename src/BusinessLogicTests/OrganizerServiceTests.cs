using Xunit;
using Moq;
using System.Collections.Generic;

using BusinessLogic.IRepositories;
using BusinessLogic.Models;
using BusinessLogic.Services;
using BusinessLogic.Exceptions;

namespace BusinessLogicTests
{
    public class OrganizerServiceTests
    {
        private readonly IOrganizerRepository _mockRepo;
        private readonly List<Organizer> _mockOrganizers;
        private readonly IOrganizerService _service;

        public OrganizerServiceTests()
        {
            _mockOrganizers = new List<Organizer>
            {
                new Organizer
                {
                    ID = 1,
                    Name = "Мак",
                    Address = "Москва",
                    Email = null,
                    URL = "www.mac.ru",
                    PhoneNumber = "123",
                },
                new Organizer
                {
                    ID = 2,
                    Name = "Посиделки",
                    Address = "Санкт-Петербург",
                    Email = "hotline@sithere.ru",
                    URL = null,
                    PhoneNumber = "321",
                },
                new Organizer
                {
                    ID = 3,
                    Name = "Мой дом",
                    Address = "Зеленоград",
                    Email = "myhome@yandex.ru",
                    URL = "www.myhome.ru",
                    PhoneNumber = null,
                },
                new Organizer
                {
                    ID = 4,
                    Name = "Nice to meet you",
                    Address = "Москва",
                    Email = "nice@nice.com",
                    URL = "www.nice.com",
                    PhoneNumber = "312",
                }
            };

            var mockRepo = new Mock<IOrganizerRepository>();
            mockRepo.Setup(repo => repo.GetAll()).Returns(_mockOrganizers);
            mockRepo.Setup(repo => repo.Add(It.IsAny<Organizer>())).Callback(
                (Organizer organizer) =>
                {
                    organizer.ID = _mockOrganizers.Count + 1;
                    _mockOrganizers.Add(organizer);
                }
                );
            mockRepo.Setup(repo => repo.GetByID(It.IsAny<long>())).Returns(
                (long id) => _mockOrganizers.Find(x => x.ID == id));
            mockRepo.Setup(repo => repo.Update(It.IsAny<Organizer>())).Callback(
                (Organizer organizer) =>
                {
                    _mockOrganizers
                        .FindAll(x => x.ID == organizer.ID)
                        .ForEach(x =>
                        {
                            x.Name = organizer.Name;
                            x.Address = organizer.Address;
                            x.Email = organizer?.Email;
                            x.URL = organizer?.URL;
                            x.PhoneNumber = organizer?.PhoneNumber;
                        });
                }
                );
            mockRepo.Setup(repo => repo.Delete(It.IsAny<Organizer>())).Callback(
                (Organizer organizer) => _mockOrganizers.RemoveAll(x => x.ID == organizer.ID));

            _mockRepo = mockRepo.Object;
            _service = new OrganizerService(_mockRepo);
        }

        [Fact]
        public void GetOrganizersTest()
        {
            var expectedCount = _mockOrganizers.Count;

            var res = _service.GetOrganizers();

            Assert.IsType<List<Organizer>>(res);
            Assert.Equal(expectedCount, res.Count);
            Assert.All(res, item => Assert.InRange(item.ID, low: 1, high: expectedCount));
        }

        [Fact]
        public void CreateOrganizerTest()
        {
            var expectedCount = _mockOrganizers.Count;
            var expectedCount2 = expectedCount + 1;
            var res = _service.GetOrganizers();
            var organizer = new Organizer
            {
                Name = "new",
                Address = "new",
            };

            Assert.Equal(expectedCount, res.Count);

            _service.CreateOrganizer(organizer);

            res = _service.GetOrganizers();

            Assert.Equal(expectedCount2, res.Count);
            Assert.All(res, item => Assert.InRange(item.ID, low: 1, high: expectedCount2));
        }

        [Fact]
        public void ThrowAlreadyExistsExcCreateOrganizerTest()
        {
            var organizer = new Organizer
            {
                Name = "Посиделки",
                Address = "Санкт-Петербург"
            };

            void action() => _service.CreateOrganizer(organizer);

            Assert.Throws<AlreadyExistsOrganizerException>(action);
        }

        [Fact]
        public void UpdateOrganizerTest()
        {
            var expectedCount = _mockOrganizers.Count;
            var organizer = new Organizer
            {
                ID = 1,
                Name = "Мак",
                Address = "new",
                Email = null,
                URL = "www.mac.ru",
                PhoneNumber = "123",
            };

            var res = _service.GetOrganizers();
            Assert.Equal(expectedCount, res.Count);

            _service.UpdateOrganizer(organizer);

            res = _service.GetOrganizers();

            Assert.Equal(expectedCount, res.Count);
            Assert.All(res, item => Assert.InRange(item.ID, low: 1, high: expectedCount));
            var newVal = res.Find(item => item.ID == organizer.ID);
            Assert.Equal(newVal?.ID, organizer.ID);
            Assert.Equal(newVal?.Name, organizer.Name);
            Assert.Equal(newVal?.Address, organizer.Address);
            Assert.Equal(newVal?.Email, organizer.Email);
            Assert.Equal(newVal?.URL, organizer.URL);
            Assert.Equal(newVal?.PhoneNumber, organizer.PhoneNumber);
        }

        [Fact]
        public void ThrowNotExistsExcUpdateOrganizerTest()
        {
            var organizer = new Organizer { ID = 100 };

            void action() => _service.UpdateOrganizer(organizer);

            Assert.Throws<NotExistsOrganizerException>(action);
        }

        [Fact]
        public void DeleteOrganizerTest()
        {
            var expectedCount = _mockOrganizers.Count;
            var organizer = new Organizer { ID = 1 };

            var res = _service.GetOrganizers();
            Assert.Equal(expectedCount, res.Count);

            _service.DeleteOrganizer(organizer);

            res = _service.GetOrganizers();

            Assert.Equal(expectedCount - 1, res.Count);
            Assert.Null(_mockOrganizers.Find(x => x.ID == organizer.ID));
        }

        [Fact]
        public void ThrowNotExistsExcDeleteOrganizerTest()
        {
            var organizer = new Organizer { ID = 100 };

            void action() => _service.DeleteOrganizer(organizer);

            Assert.Throws<NotExistsOrganizerException>(action);
        }
    }
}
