using Xunit;
using Moq;
using System.Collections.Generic;

using BusinessLogic.IRepositories;
using BusinessLogic.Models;
using BusinessLogic.Services;

namespace BusinessLogicTests
{
    public class VenueServiceTests
    {
        [Fact]
        public void GetVenuesTest()
        {
            var mockRepo = new Mock<IVenueRepository>();
            mockRepo.Setup(repo => repo.GetAll()).Returns(GetAllVenues());
            var service = new VenueService(mockRepo.Object);
            var expectedCount = GetAllVenues().Count;

            var res = service.GetVenues();

            Assert.IsType<List<Venue>>(res);
            Assert.Equal(expectedCount, res.Count);
            Assert.All(res, item => Assert.InRange(item.ID, low: 1, high: expectedCount));
        }

        private List<Venue> GetAllVenues()
        {
            var venues = new List<Venue>
            {
                new Venue
                {
                    ID = 1,
                    Name = "Мак",
                    Type = "фастфуд",
                    Address = "Москва",
                    Email = null,
                    URL = "www.mac.ru",
                    PhoneNumber = "123",
                },
                new Venue
                {
                    ID = 2,
                    Name = "Посиделки",
                    Type = "анти-кафе",
                    Address = "Санкт-Петербург",
                    Email = "hotline@sithere.ru",
                    URL = null,
                    PhoneNumber = "321",
                },
                new Venue
                {
                    ID = 3,
                    Name = "Мой дом",
                    Type = "студия",
                    Address = "Зеленоград",
                    Email = "myhome@yandex.ru",
                    URL = "www.myhome.ru",
                    PhoneNumber = null,
                },
                new Venue
                {
                    ID = 4,
                    Name = "Nice to meet you",
                    Type = "кафе",
                    Address = "Москва",
                    Email = "nice@nice.com",
                    URL = "www.nice.com",
                    PhoneNumber = "312",
                }
            };

            return venues;
        }
    }
}
