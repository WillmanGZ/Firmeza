using Microsoft.Extensions.DependencyInjection;
using Firmeza.API.Configs;
using Firmeza.API.Data;

namespace Firmeza.Tests
{
    public class ApiTests
    {
        [Fact]
        public void AddDatabaseShouldRegisterAppDbContext()
        {
            // Arrange
            var services = new ServiceCollection();

            // Fijamos variables falsas para evitar conectarse a una DB real
            Environment.SetEnvironmentVariable("DB_HOST", "localhost");
            Environment.SetEnvironmentVariable("DB_PORT", "5432");
            Environment.SetEnvironmentVariable("DB_NAME", "testdb");
            Environment.SetEnvironmentVariable("DB_USER", "user");
            Environment.SetEnvironmentVariable("DB_PASS", "password");

            // Act
            services.AddDatabase();
            var provider = services.BuildServiceProvider();

            var dbContext = provider.GetService<AppDbContext>();

            // Assert
            Assert.NotNull(dbContext);
        }


        [Fact]
        public void DatabaseShouldConnectSuccessfully()
        {
            // Arrange
            var services = new ServiceCollection();

            // Se usan las variables de entorno REALES de tu entorno
            // La API ya debería tenerlas configuradas

            services.AddDatabase();
            var provider = services.BuildServiceProvider();

            var context = provider.GetRequiredService<AppDbContext>();

            // Act
            var canConnect = context.Database.CanConnect();

            // Assert
            Assert.True(canConnect, "La base de datos NO pudo conectarse.");
        }
    }
}
