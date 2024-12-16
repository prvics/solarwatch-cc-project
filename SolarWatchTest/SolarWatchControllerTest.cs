using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SolarWatch.Controllers;
using SolarWatch.Models;
using SolarWatch.Services;
using Microsoft.Extensions.Logging;

namespace SolarWatch.Tests.Controllers
{
    [TestFixture]
    public class SolarWatchControllerTest
    {
        private Mock<ISolarWatchService> _mockService;
        private Mock<ILogger<SolarWatchController>> _mockLogger;
        private SolarWatchController _controller;

        [SetUp]
        public void SetUp()
        {
            _mockService = new Mock<ISolarWatchService>();
            _mockLogger = new Mock<ILogger<SolarWatchController>>();
            _controller = new SolarWatchController(_mockService.Object, _mockLogger.Object);
        }

        [Test]
        public async Task GetSunriseSunset_ShouldReturnNotFound_WhenDataIsUnavailable()
        {
            // Arrange
            var city = "Unknown City";
            var date = DateTime.Now;

            _mockService.Setup(s => s.GetSunriseSunset(city, date))
                        .ThrowsAsync(new Exception("Data not found"));

            // Act
            var result = await _controller.GetSunriseSunset(city, date) as NotFoundObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(404));
            Assert.That(result.Value, Is.EqualTo("Unable to retrieve data"));
        }

        [Test]
        public async Task CreateCity_ShouldReturnCreatedAtAction_WhenCityIsCreated()
        {
            // Arrange
            var newCity = new CityModel
            {
                CityId = 1,
                City = "New City",
                Lat = 10.0,
                Lon = 20.0,
                Country = "New Country",
                State = "New State"
            };

            _mockService.Setup(s => s.CreateCity(newCity))
                        .ReturnsAsync(newCity);

            // Act
            var result = await _controller.CreateCity(newCity) as CreatedAtActionResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(201));
            Assert.That(result.ActionName, Is.EqualTo("GetCityById"));
            Assert.That(result.Value, Is.EqualTo(newCity));
        }
    }
}