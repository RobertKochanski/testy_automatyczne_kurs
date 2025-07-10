using FluentAssertions;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RestaurantAPI.Entities;
using RestaurantAPI.IntegrationTests.Helpers;
using RestaurantAPI.Models;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace RestaurantAPI.IntegrationTests
{
    public class RestaurantControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private HttpClient _httpClient;
        private WebApplicationFactory<Startup> _factory;

        public RestaurantControllerTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        var dvContextOptions = services.SingleOrDefault(service => service.ServiceType == typeof(DbContextOptions<RestaurantDbContext>));

                        services.Remove(dvContextOptions);

                        services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();

                        services.AddMvc(option => option.Filters.Add(new FakeUserFilter()));

                        services.AddDbContext<RestaurantDbContext>(options => options.UseInMemoryDatabase("RestaurantDb"));
                    });
                });
            _httpClient = _factory.CreateClient();
        }

        private void SeedRestaurant(Restaurant restaurant)
        {
            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var _dbContext = scope.ServiceProvider.GetService<RestaurantDbContext>();

            _dbContext.Restaurants.Add(restaurant);
            _dbContext.SaveChanges();
        }

        [Fact]
        public async Task Delete_ForNonRestaurantsOwner_ReturnsForbidden()
        {
            // arrange

            var restaurant = new Restaurant()
            {
                CreatedById = 1000,
                Name = "Test"
            };

            SeedRestaurant(restaurant);

            // act

            var response = await _httpClient.DeleteAsync("/api/restaurant/" + restaurant.Id);

            // assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Delete_ForRestaurantsOwner_ReturnsNoContent()
        {
            // arrange

            var restaurant = new Restaurant()
            {
                CreatedById = 1,
                Name = "Test"
            };

            SeedRestaurant(restaurant);

            // act

            var response = await _httpClient.DeleteAsync("/api/restaurant/" + restaurant.Id);

            // assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Delete_ForNonExistintRestaurant_ReturnsNotFound()
        {
            // arrange

            // act

            var response = await _httpClient.DeleteAsync("/api/restaurant/987");

            // assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task CreateRestaurant_WithValidModel_ReturnsCreatedStatus()
        {
            // arrange
            var model = new CreateRestaurantDto()
            {
                Name = "TestRestaurant",
                City = "Kraków",
                Street = "Długa 5"
            };

            var httpContent = model.ToJsonHttpContent();

            // act
            var response = await _httpClient.PostAsync("/api/restaurant", httpContent);

            // assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
            response.Headers.Location.Should().NotBeNull();
        }

        [Fact]
        public async Task CreateRestaurant_WithInvalidModel_ReturnsCreatedStatus()
        {
            // arrange
            var model = new CreateRestaurantDto()
            {
                ContactEmail = "test@test.com",
                Description = "Description",
                ContactNumber = "111 222 333",
            };

            var httpContent = model.ToJsonHttpContent();

            // act
            var response = await _httpClient.PostAsync("/api/restaurant", httpContent);

            // assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }


        [Theory]
        [InlineData("pageSize=5&pageNumber=1")]
        [InlineData("pageSize=15&pageNumber=2")]
        [InlineData("pageSize=10&pageNumber=3")]
        public async Task GetAll_WithQueryParameters_ReturnsOkResult(string queryParams)
        {
            // act 
            var response = await _httpClient.GetAsync("/api/restaurant?" + queryParams);

            // assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Theory]
        [InlineData("pageSize=125&pageNumber=5")]
        [InlineData("pageSize=135&pageNumber=2")]
        [InlineData(null)]
        public async Task GetAll_WithInvalidQueryParams_ReturnsBadRequest(string queryParams)
        {
            // act 
            var response = await _httpClient.GetAsync("/api/restaurant?" + queryParams);

            // assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }
    }
}
