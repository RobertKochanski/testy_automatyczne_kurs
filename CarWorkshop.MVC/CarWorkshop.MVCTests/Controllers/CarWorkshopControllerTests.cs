using CarWorkshop.Application.CarWorkshop;
using CarWorkshop.Application.CarWorkshop.Queries.GetAllCarWorkshops;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Moq;
using Xunit;

namespace CarWorkshop.MVCTests.Controllers
{
    public class CarWorkshopControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public CarWorkshopControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Index_ReturnsViewWithExpectedData_ForExistingWorkshop()
        {
            // arrange

            var carWorkshops = new List<CarWorkshopDto>() 
            {
                new CarWorkshopDto()
                {
                    Name = "Workshop 1",
                },
                new CarWorkshopDto()
                {
                    Name = "Workshop 2",
                },
                new CarWorkshopDto()
                {
                    Name = "Workshop 3",
                },
            };

            var mediatorMock = new Mock<IMediator>();

            mediatorMock.Setup(m => 
            m.Send(It.IsAny<GetAllCarWorkshopsQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(carWorkshops);

            var client = _factory
                .WithWebHostBuilder(builder => 
                    builder.ConfigureTestServices(services =>
                    services.AddScoped(_ => mediatorMock.Object)))
                .CreateClient();

            // act

            var response = await client.GetAsync("/CarWorkshop/Index");

            // assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var content = await response.Content.ReadAsStringAsync();

            content.Should().Contain("<h1>Car Workshops</h1>")
                .And.Contain("Workshop 1")
                .And.Contain("Workshop 2")
                .And.Contain("Workshop 3");
        }

        [Fact]
        public async Task Index_ReturnsEmptyView_ForNoCarWorkshopsExist()
        {
            // arrange

            var carWorkshops = new List<CarWorkshopDto>(){};

            var mediatorMock = new Mock<IMediator>();

            mediatorMock.Setup(m =>
            m.Send(It.IsAny<GetAllCarWorkshopsQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(carWorkshops);

            var client = _factory
                .WithWebHostBuilder(builder =>
                    builder.ConfigureTestServices(services =>
                    services.AddScoped(_ => mediatorMock.Object)))
                .CreateClient();

            // act

            var response = await client.GetAsync("/CarWorkshop/Index");

            // assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var content = await response.Content.ReadAsStringAsync();

            content.Should().NotContain("div class=\"card m-3\"");
        }
    }
}
