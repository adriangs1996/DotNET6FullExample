using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Domain;
using Domain.Entities;
using Domain.Mocks;
using Domain.Models.Inputs;
using Domain.Models.Outputs;
using FluentAssertions;
using HiringTest.Controllers;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using Moq;
using Services.Contracts;

namespace HiringTest.Test.Tests.Controllers
{
    public class TestEntityControllerTest
    {
        private IMapper ConfigureMapper()
        {
            var mapperConfig = new MapperConfiguration(mc => { mc.AddProfile(new DomainMappingProfile()); });
            IMapper mapper = mapperConfig.CreateMapper();
            return mapper;
        }

        [Fact]
        public async Task GetAllEntities_ShouldReturn200StatusAndTestEntitiesDetails()
        {
            // Arrange
            var mapper = ConfigureMapper();
            var service = new Mock<IApplicationService>();
            var notifications = new Mock<INotificationService>();
            var entities = TestEntityDetailsMock.MockTestEntitiesDetails(5);
            service.Setup(_ => _.GetAllTestEntities()).ReturnsAsync(entities);
            var systemUnderTest = new TestEntityController(mapper, service.Object, notifications.Object);

            // Act
            var result = await systemUnderTest.GetAllTestEntities();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));
            (result as OkObjectResult).StatusCode.Should().Be(200);
            (result as OkObjectResult).Value.Should().BeOfType(typeof(List<TestEntityDetails>));
        }

        [Fact]
        public async Task GetTestEntityDetails_ShouldReturn404StatusWhenInvalidId()
        {
            // Arrange
            var mapper = ConfigureMapper();
            var service = new Mock<IApplicationService>();
            var notifications = new Mock<INotificationService>();
            service.Setup(_ => _.GetTestEntityById(It.IsAny<long>()))
                .ReturnsAsync(default(TestEntity?));
            var systemUnderTest = new TestEntityController(mapper, service.Object, notifications.Object);

            // Act
            var result = await systemUnderTest.GetTestEntityDetails(10);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(NotFoundResult));
            (result as NotFoundResult).StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task CreateNewTestEntity_CallsNotificationServiceOnSuccess()
        {
            // Arrange
            var mapper = ConfigureMapper();
            var service = new Mock<IApplicationService>();
            var notifications = new Mock<INotificationService>();
            notifications.Setup(_ => _.TestEntityAdded(It.IsAny<TestEntityDetails>()));
            service.Setup(_ => _.CreateTestEntity(It.IsAny<TestEntityInDto>()))
                .ReturnsAsync(TestEntityDetailsMock.GetOne());
            var systemUnderTest = new TestEntityController(mapper, service.Object, notifications.Object);

            // Act
            var result = await systemUnderTest.CreateNewTestEntity(new() {IsComplete = true, Name = "test"});

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(CreatedAtActionResult));
            notifications.Verify(m => m.TestEntityAdded(It.IsAny<TestEntityDetails>()), Times.Once());
        }

        [Fact]
        public async Task UpdateTestEntity_ShouldCallNotificationService()
        {
            // Arrange
            var mapper = ConfigureMapper();
            var service = new Mock<IApplicationService>();
            var notifications = new Mock<INotificationService>();
            notifications.Setup(_ => _.TestEntityEdited(It.IsAny<TestEntityDetails>()));
            service.Setup(_ => _.UpdateEntityIfPresent(It.IsAny<long>(), It.IsAny<TestEntityInDto>()))
                .ReturnsAsync(TestEntityDetailsMock.GetOne());
            var systemUnderTest = new TestEntityController(mapper, service.Object, notifications.Object);

            // Act
            var result = await systemUnderTest.UpdateTestEntity(1, new());

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(AcceptedAtActionResult));
            notifications.Verify(m => m.TestEntityEdited(It.IsAny<TestEntityDetails>()), Times.Once());
        }

        [Fact]
        public async Task DeleteTestEntity_ShouldNotify()
        {
            // Arrange
            var mapper = ConfigureMapper();
            var service = new Mock<IApplicationService>();
            var notifications = new Mock<INotificationService>();
            notifications.Setup(_ => _.TestEntityRemoved(It.IsAny<TestEntityDetails>()));
            service.Setup(_ => _.DeleteTestEntity(It.IsAny<long>()))
                .ReturnsAsync(TestEntityDetailsMock.GetOne());
            var systemUnderTest = new TestEntityController(mapper, service.Object, notifications.Object);

            // Act
            var result = await systemUnderTest.DeleteTestEntity(10);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(AcceptedResult));
            notifications.Verify(m => m.TestEntityRemoved(It.IsAny<TestEntityDetails>()), Times.Once());
        }
    }
}