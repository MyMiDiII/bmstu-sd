using Xunit;
using Moq;
using System.Collections.Generic;

using BusinessLogic.IRepositories;
using BusinessLogic.Models;
using BusinessLogic.Services;
using BusinessLogic.Exceptions;

namespace BusinessLogicTests
{
    public class VenueServiceTests
    {
        private readonly IVenueRepository _mockRepo;
        private readonly IVenueService _service;

        private readonly List<Venue> _mockVenues;

        public VenueServiceTests()
        {
            _mockVenues = new List<Venue>
            {
                new Venue("Мак", "фастфуд", "Москва")
                {
                    ID = 1,
                    Email = null,
                    URL = "www.mac.ru",
                    PhoneNumber = "123",
                },
                new Venue("Посиделки", "анти-кафе", "Санкт-Петербург")
                {
                    ID = 2,
                    Email = "hotline@sithere.ru",
                    URL = null,
                    PhoneNumber = "321",
                },
                new Venue("Мой дом", "студия", "Зеленоград")
                {
                    ID = 3,
                    Email = "myhome@yandex.ru",
                    URL = "www.myhome.ru",
                    PhoneNumber = null,
                },
                new Venue("Nice to meet you", "кафе", "Москва")
                {
                    ID = 4,
                    Email = "nice@nice.com",
                    URL = "www.nice.com",
                    PhoneNumber = "312",
                }
            };

            var mockRepo = new Mock<IVenueRepository>();
            mockRepo.Setup(repo => repo.GetAll()).Returns(_mockVenues);
            mockRepo.Setup(repo => repo.Add(It.IsAny<Venue>())).Callback(
                (Venue venue) =>
                {
                    venue.ID = _mockVenues.Count + 1;
                    _mockVenues.Add(venue);
                }
                );
            mockRepo.Setup(repo => repo.GetByID(It.IsAny<long>())).Returns(
                (long id) => _mockVenues.Find(x => x.ID == id));
            mockRepo.Setup(repo => repo.Update(It.IsAny<Venue>())).Callback(
                (Venue venue) =>
                {
                    _mockVenues
                        .FindAll(x => x.ID == venue.ID)
                        .ForEach(x =>
                        {
                            x.Name = venue.Name;
                            x.Type = venue.Type;
                            x.Address = venue.Address;
                            x.Email = venue?.Email;
                            x.URL = venue?.URL;
                            x.PhoneNumber = venue?.PhoneNumber;
                        });
                }
                );
            mockRepo.Setup(repo => repo.Delete(It.IsAny<Venue>())).Callback(
                (Venue venue) => _mockVenues.RemoveAll(x => x.ID == venue.ID));

            _mockRepo = mockRepo.Object;
            _service = new VenueService(_mockRepo);
        }

        [Fact]
        public void GetVenuesTest()
        {
            var expectedCount = _mockVenues.Count;

            var res = _service.GetVenues();

            Assert.IsType<List<Venue>>(res);
            Assert.Equal(expectedCount, res.Count);
            Assert.All(res, item => Assert.InRange(item.ID, low: 1, high: expectedCount));
        }

        [Fact]
        public void CreateVenueTest()
        {
            var expectedCount = _mockVenues.Count;
            var expectedCount2 = expectedCount + 1;
            var res = _service.GetVenues();
            var venue = new Venue("new", "new", "new");

            Assert.Equal(expectedCount, res.Count);

            _service.CreateVenue(venue);

            res = _service.GetVenues();

            Assert.Equal(expectedCount2, res.Count);
            Assert.All(res, item => Assert.InRange(item.ID, low: 1, high: expectedCount2));
        }

        [Fact]
        public void ThrowAlreadyExistsExcCreateVenueTest()
        {
            var venue = new Venue("Посиделки", "анти-кафе", "Санкт-Петербург");

            void action() => _service.CreateVenue(venue);

            Assert.Throws<AlreadyExistsVenueException>(action);
        }

        [Fact]
        public void UpdateVenueTest()
        {
            var expectedCount = _mockVenues.Count;
            var venue = new Venue("Мак", "new", "new")
            {
                ID = 1,
                Email = null,
                URL = "www.mac.ru",
                PhoneNumber = "123",
            };

            var res = _service.GetVenues();
            Assert.Equal(expectedCount, res.Count);

            _service.UpdateVenue(venue);

            res = _service.GetVenues();

            Assert.Equal(expectedCount, res.Count);
            Assert.All(res, item => Assert.InRange(item.ID, low: 1, high: expectedCount));
            var newVal = res.Find(item => item.ID == venue.ID);
            Assert.Equal(newVal?.ID, venue.ID);
            Assert.Equal(newVal?.Name, venue.Name);
            Assert.Equal(newVal?.Address, venue.Address);
            Assert.Equal(newVal?.Type, venue.Type);
            Assert.Equal(newVal?.Email, venue.Email);
            Assert.Equal(newVal?.URL, venue.URL);
            Assert.Equal(newVal?.PhoneNumber, venue.PhoneNumber);
        }

        [Fact]
        public void ThrowNotExistsExcUpdateVenueTest()
        {
            var venue = new Venue("new", "new", "new") { ID = 100 };

            void action() => _service.UpdateVenue(venue);

            Assert.Throws<NotExistsVenueException>(action);
        }

        [Fact]
        public void DeleteVenueTest()
        {
            var expectedCount = _mockVenues.Count;
            var venue = new Venue("new", "new", "new") { ID = 1 };

            var res = _service.GetVenues();
            Assert.Equal(expectedCount, res.Count);

            _service.DeleteVenue(venue);

            res = _service.GetVenues();

            Assert.Equal(expectedCount - 1, res.Count);
            Assert.Null(_mockVenues.Find(x => x.ID == venue.ID));
        }

        [Fact]
        public void ThrowNotExistsExcDeleteVenueTest()
        {
            var venue = new Venue("new", "new", "new") { ID = 100 };

            void action() => _service.DeleteVenue(venue);

            Assert.Throws<NotExistsVenueException>(action);
        }
    }
}
