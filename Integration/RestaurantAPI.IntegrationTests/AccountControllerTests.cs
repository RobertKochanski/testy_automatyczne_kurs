using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using RestaurantAPI.Entities;
using RestaurantAPI.IntegrationTests.Helpers;
using RestaurantAPI.Models;
using RestaurantAPI.Services;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace RestaurantAPI.IntegrationTests
{
    public class AccountControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private HttpClient _httpClient;
        private Mock<IAccountService> _accountServiceMock = new Mock<IAccountService>();

        public AccountControllerTests(WebApplicationFactory<Startup> factory)
        {
            _httpClient = factory
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        var dvContextOptions = services.SingleOrDefault(service => service.ServiceType == typeof(DbContextOptions<RestaurantDbContext>));

                        services.Remove(dvContextOptions);

                        services.AddSingleton<IAccountService>(_accountServiceMock.Object);

                        services.AddDbContext<RestaurantDbContext>(options => options.UseInMemoryDatabase("RestaurantDb"));
                    });
                })
                .CreateClient();
        }

        [Fact]
        public async Task LoginUser_ForRegisteredUser_ReturnsOk()
        {
            // arrange

            _accountServiceMock
                .Setup(e => e.GenerateJwt(It.IsAny<LoginDto>()))
                .Returns("jwt");

            var loginUser = new LoginDto()
            {
                Email = "test@test.com",
                Password = "password123",
            };

            var httpContent = loginUser.ToJsonHttpContent();

            // act

            var response = await _httpClient.PostAsync("/api/account/login", httpContent);

            // assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task RegisterUser_ForValidModel_ReturnsOk()
        {
            // arrange

            var registerUser = new RegisterUserDto()
            {
                Email = "test@test.com",
                Password = "password123",
                ConfirmPassword = "password123"
            };

            var httpContent = registerUser.ToJsonHttpContent();

            // act

            var response = await _httpClient.PostAsync("/api/account/register", httpContent);

            // assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task RegisterUser_ForInvalidModel_ReturnsBadRequest()
        {
            // arrange

            var registerUser = new RegisterUserDto()
            {
                Password = "password123",
                ConfirmPassword = "password1234"
            };

            var httpContent = registerUser.ToJsonHttpContent();

            // act

            var response = await _httpClient.PostAsync("/api/account/register", httpContent);

            // assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }
    }
}
