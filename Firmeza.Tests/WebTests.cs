using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Firmeza.API.Data;

namespace Firmeza.Tests
{
    public class WebTests : IClassFixture<WebApplicationFactory<TestHost>>
    {
        private readonly WebApplicationFactory<TestHost> _factory;

        public WebTests(WebApplicationFactory<TestHost> factory)
        {
            _factory = factory;
        }

        [Fact]
        public void RazorShouldStartSuccessfully()
        {
            // Si el cliente puede crearse, el host arranco
            var client = _factory.CreateClient();
            Assert.NotNull(client);
        }

        [Fact]
        public void RazorShouldConnectToDatabase()
        {
            using var scope = _factory.Services.CreateScope();

            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var canConnect = db.Database.CanConnect();

            Assert.True(canConnect, "El Razor Pages NO pudo conectarse a la base de datos.");
        }
    }
}
