using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using RestaurantAPI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace RestaurantAPI.IntegrationTests
{
    public class StartupTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly List<Type> _controllersTypes;
        private readonly WebApplicationFactory<Startup> _factory;

        public StartupTests(WebApplicationFactory<Startup> factory)
        {
            _controllersTypes = typeof(Startup)
                .Assembly
                .GetTypes()
                .Where(t => t.IsSubclassOf(typeof(ControllerBase)))
                .ToList();

            _factory = factory.WithWebHostBuilder(builder => 
            {
                builder.ConfigureServices(services =>
                {
                    _controllersTypes.ForEach(c => services.AddScoped(c));
                });
            });
        }

        [Fact]
        public void ConfigureServices_ForControllers_RegistersAllDependencies()
        {
            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();

            var controller = scope.ServiceProvider.GetService<AccountController>();

            // assert

            _controllersTypes.ForEach(t =>
            {
                var controller = scope.ServiceProvider.GetService(t);
                controller.Should().NotBeNull();
            });
        }
    }
}
