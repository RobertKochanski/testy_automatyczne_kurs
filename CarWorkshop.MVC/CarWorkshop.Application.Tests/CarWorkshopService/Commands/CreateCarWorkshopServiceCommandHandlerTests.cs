using CarWorkshop.Application.ApplicationUser;
using CarWorkshop.Application.CarWorkshop;
using CarWorkshop.Application.CarWorkshopService.Commands;
using CarWorkshop.Domain.Entities;
using CarWorkshop.Domain.Interfaces;
using FluentAssertions;
using MediatR;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarWorkshop.Application.Tests.CarWorkshopService.Commands
{
    public class CreateCarWorkshopServiceCommandHandlerTests
    {
        [Fact]
        public async Task Handle_CreatesCarWorkshopService_WhenUserIsAuthorize()
        {
            // arrange

            var carWorkshop = new CarWorkshopEntity()
            {
                Id = 1,
                CreatedById = "1"
            };

            var command = new CreateCarWorkshopServiceCommand()
            {
                Cost = "100 PLN",
                Description = "Service Description",
                CarWorkshopEncodedName = "workshop1"
            };

            var userContextMock = new Mock<IUserContext>();

            userContextMock.Setup(c => c.GetCurrentUser()).Returns(new CurrentUser("1", "test@test.com", new[] {"User"}));

            var carWorkshopRepositoryMock = new Mock<ICarWorkshopRepository>();

            carWorkshopRepositoryMock.Setup(c => c.GetByEncodedName(command.CarWorkshopEncodedName)).ReturnsAsync(carWorkshop);

            var carWorkshopServiceRepositoryMock = new Mock<ICarWorkshopServiceRepository>();

            var handler = new CreateCarWorkshopServiceCommandHandler(userContextMock.Object, carWorkshopRepositoryMock.Object, carWorkshopServiceRepositoryMock.Object);

            // act

            var result = await handler.Handle(command, CancellationToken.None);

            // assert

            result.Should().Be(Unit.Value);
            carWorkshopServiceRepositoryMock.Verify(m => m.Create(It.IsAny<Domain.Entities.CarWorkshopService>()), Times.Once);
        }

        [Fact]
        public async Task Handle_CreatesCarWorkshopService_WhenUserIsModerator()
        {
            // arrange

            var carWorkshop = new CarWorkshopEntity()
            {
                Id = 1,
                CreatedById = "1"
            };

            var command = new CreateCarWorkshopServiceCommand()
            {
                Cost = "100 PLN",
                Description = "Service Description",
                CarWorkshopEncodedName = "workshop1"
            };

            var userContextMock = new Mock<IUserContext>();

            userContextMock.Setup(c => c.GetCurrentUser()).Returns(new CurrentUser("2", "test@test.com", new[] { "Moderator" }));

            var carWorkshopRepositoryMock = new Mock<ICarWorkshopRepository>();

            carWorkshopRepositoryMock.Setup(c => c.GetByEncodedName(command.CarWorkshopEncodedName)).ReturnsAsync(carWorkshop);

            var carWorkshopServiceRepositoryMock = new Mock<ICarWorkshopServiceRepository>();

            var handler = new CreateCarWorkshopServiceCommandHandler(userContextMock.Object, carWorkshopRepositoryMock.Object, carWorkshopServiceRepositoryMock.Object);

            // act

            var result = await handler.Handle(command, CancellationToken.None);

            // assert

            result.Should().Be(Unit.Value);
            carWorkshopServiceRepositoryMock.Verify(m => m.Create(It.IsAny<Domain.Entities.CarWorkshopService>()), Times.Once);
        }

        [Fact]
        public async Task Handle_DoesntCreatesCarWorkshopService_WhenUserIsNotAuthorize()
        {
            // arrange

            var carWorkshop = new CarWorkshopEntity()
            {
                Id = 1,
                CreatedById = "1"
            };

            var command = new CreateCarWorkshopServiceCommand()
            {
                Cost = "100 PLN",
                Description = "Service Description",
                CarWorkshopEncodedName = "workshop1"
            };

            var userContextMock = new Mock<IUserContext>();

            userContextMock.Setup(c => c.GetCurrentUser()).Returns(new CurrentUser("2", "test@test.com", new[] { "User" }));

            var carWorkshopRepositoryMock = new Mock<ICarWorkshopRepository>();

            carWorkshopRepositoryMock.Setup(c => c.GetByEncodedName(command.CarWorkshopEncodedName)).ReturnsAsync(carWorkshop);

            var carWorkshopServiceRepositoryMock = new Mock<ICarWorkshopServiceRepository>();

            var handler = new CreateCarWorkshopServiceCommandHandler(userContextMock.Object, carWorkshopRepositoryMock.Object, carWorkshopServiceRepositoryMock.Object);

            // act

            var result = await handler.Handle(command, CancellationToken.None);

            // assert

            result.Should().Be(Unit.Value);
            carWorkshopServiceRepositoryMock.Verify(m => m.Create(It.IsAny<Domain.Entities.CarWorkshopService>()), Times.Never);
        }

        [Fact]
        public async Task Handle_DoesntCreatesCarWorkshopService_WhenUserIsAuthenticated()
        {
            // arrange

            var carWorkshop = new CarWorkshopEntity()
            {
                Id = 1,
                CreatedById = "1"
            };

            var command = new CreateCarWorkshopServiceCommand()
            {
                Cost = "100 PLN",
                Description = "Service Description",
                CarWorkshopEncodedName = "workshop1"
            };

            var userContextMock = new Mock<IUserContext>();

            userContextMock.Setup(c => c.GetCurrentUser()).Returns((CurrentUser?)null);

            var carWorkshopRepositoryMock = new Mock<ICarWorkshopRepository>();

            carWorkshopRepositoryMock.Setup(c => c.GetByEncodedName(command.CarWorkshopEncodedName)).ReturnsAsync(carWorkshop);

            var carWorkshopServiceRepositoryMock = new Mock<ICarWorkshopServiceRepository>();

            var handler = new CreateCarWorkshopServiceCommandHandler(userContextMock.Object, carWorkshopRepositoryMock.Object, carWorkshopServiceRepositoryMock.Object);

            // act

            var result = await handler.Handle(command, CancellationToken.None);

            // assert

            result.Should().Be(Unit.Value);
            carWorkshopServiceRepositoryMock.Verify(m => m.Create(It.IsAny<Domain.Entities.CarWorkshopService>()), Times.Never);
        }
    }
}
