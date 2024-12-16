using Moq;
using NUnit.Framework;
using SolarWatch.Models;
using SolarWatch.Models.SunriseSunsetDto;
using SolarWatch.Services;
using SolarWatch.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using NUnit.Framework.Legacy;

namespace SolarWatch.Tests.Services
{
    [TestFixture]
    public class SolarWatchServiceTests
    {
        private SolarWatchService _solarWatchService;
        private Mock<IJsonProcessor> _jsonProcessorMock;
        private SolarWatchContext _context;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<SolarWatchContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new SolarWatchContext(options);
            _jsonProcessorMock = new Mock<IJsonProcessor>();
            _solarWatchService = new SolarWatchService(_jsonProcessorMock.Object, _context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task CreateCity_ShouldAddCityToDatabase()
        {
            // Arrange
            var city = new CityModel
            {
                City = "Test City",
                Lat = 10.0,
                Lon = 20.0,
                Country = "Test Country",
                State = "Test State"
            };

            // Act
            var result = await _solarWatchService.CreateCity(city);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.City, Is.EqualTo("Test City"));
            ClassicAssert.AreEqual(1, await _context.Cities.CountAsync());
        }

        [Test]
        public async Task UpdateCity_ShouldUpdateCityDetails()
        {
            // Arrange
            var city = new CityModel
            {
                CityId = 1,
                City = "Original City",
                Lat = 10.0,
                Lon = 20.0,
                Country = "Original Country",
                State = "Original State"
            };
            await _context.Cities.AddAsync(city);
            await _context.SaveChangesAsync();

            var updatedCity = new CityModel
            {
                CityId = 1,
                City = "Updated City",
                Lat = 15.0,
                Lon = 25.0,
                Country = "Updated Country",
                State = "Updated State"
            };

            // Act
            var result = await _solarWatchService.UpdateCity(1, updatedCity);

            // Assert
            ClassicAssert.NotNull(result);
            ClassicAssert.AreEqual("Updated City", result.City);
            ClassicAssert.AreEqual(15.0, result.Lat);
        }

        [Test]
        public async Task DeleteCity_ShouldRemoveCityFromDatabase()
        {
            // Arrange
            var city = new CityModel
            {
                CityId = 1,
                City = "Test City",
                Lat = 10.0,
                Lon = 20.0,
                Country = "Test Country",
                State = "Test State"
            };
            await _context.Cities.AddAsync(city);
            await _context.SaveChangesAsync();

            // Act
            var result = await _solarWatchService.DeleteCity(1);

            // Assert
            ClassicAssert.NotNull(result);
            ClassicAssert.AreEqual("Test City", result.City);
            ClassicAssert.AreEqual(0, await _context.Cities.CountAsync());
        }

        [Test]
        public async Task GetSunriseSunset_ShouldReturnExistingRecord_WhenDataExists()
        {
            // Arrange
            var city = new CityModel
            {
                CityId = 1,
                City = "Test City",
                Lat = 10.0,
                Lon = 20.0,
                Country = "Test Country",
                State = "Test State"
            };
            var sunriseSunset = new SunsetSunriseModel
            {
                Id = 1,
                City = "Test City",
                Date = "2024-12-10",
                Sunrise = "06:00 AM",
                Sunset = "06:00 PM",
                CityId = 1,
                CityModel = city
            };
            await _context.Cities.AddAsync(city);
            await _context.SunriseSunsets.AddAsync(sunriseSunset);
            await _context.SaveChangesAsync();

            // Act
            var result = await _solarWatchService.GetSunriseSunset("Test City", DateTime.Parse("2024-12-10"));

            // Assert
            ClassicAssert.NotNull(result);
            ClassicAssert.AreEqual("06:00 AM", result.Sunrise);
        }

        [Test]
        public async Task GetSunriseSunset_ShouldFetchFromApi_WhenDataDoesNotExist()
        {
            // Arrange
            var city = new CityModel
            {
                CityId = 1,
                City = "Test City",
                Lat = 10.0,
                Lon = 20.0,
                Country = "Test Country",
                State = "Test State"
            };

            _jsonProcessorMock
                .Setup(jp => jp.Process(It.IsAny<string>(), It.IsAny<CityModel>(), It.IsAny<string>()))
                .Returns(new SunsetSunriseModel
                {
                    City = "Test City",
                    Date = "2024-12-10",
                    Sunrise = "06:30 AM",
                    Sunset = "06:30 PM",
                    CityModel = city
                });

            await _context.Cities.AddAsync(city);
            await _context.SaveChangesAsync();

            // Act
            var result = await _solarWatchService.GetSunriseSunset("Test City", DateTime.Parse("2024-12-10"));

            // Assert
            ClassicAssert.NotNull(result);
            ClassicAssert.AreEqual("06:30 AM", result.Sunrise);
        }

        [Test]
        public async Task CreateSunriseSunset_ShouldAddRecordToDatabase()
        {
            // Arrange
            var city = new CityModel
            {
                CityId = 1,
                City = "Test City",
                Lat = 10.0,
                Lon = 20.0,
                Country = "Test Country",
                State = "Test State"
            };
            var sunriseSunset = new SunsetSunriseModel
            {
                City = "Test City",
                Date = "2024-12-10",
                Sunrise = "06:00 AM",
                Sunset = "06:00 PM",
                CityId = 1,
                CityModel = city
            };
            await _context.Cities.AddAsync(city);
            await _context.SaveChangesAsync();

            // Act
            var result = await _solarWatchService.CreateSunriseSunset(sunriseSunset);

            // Assert
            ClassicAssert.NotNull(result);
            ClassicAssert.AreEqual("06:00 AM", result.Sunrise);
        }

        [Test]
        public async Task DeleteSunriseSunset_ShouldRemoveRecordFromDatabase()
        {
            // Arrange
            var sunriseSunset = new SunsetSunriseModel
            {
                Id = 1,
                City = "Test City",
                Date = "2024-12-10",
                Sunrise = "06:00 AM",
                Sunset = "06:00 PM",
                CityId = 1
            };
            await _context.SunriseSunsets.AddAsync(sunriseSunset);
            await _context.SaveChangesAsync();

            // Act
            var result = await _solarWatchService.DeleteSunriseSunset(1);

            // Assert
            ClassicAssert.NotNull(result);
            ClassicAssert.AreEqual("Test City", result.City);
            ClassicAssert.AreEqual(0, await _context.SunriseSunsets.CountAsync());
        }
    }
}