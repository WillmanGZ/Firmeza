using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using Firmeza.API.Data.Seeders;
using Firmeza.API.Data;
using Firmeza.API.Interfaces;

namespace Firmeza.Tests
{
    internal class CustomApiFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // 1. ELIMINAR configuraciones de DbContext existentes
                services.RemoveAll(typeof(DbContextOptions<AppDbContext>));
                services.RemoveAll(typeof(AppDbContext));

                // 2. CONFIGURAR DbContext para usar una Base de Datos en Memoria (InMemory)
                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDatabaseInMemory");
                });

                // 3. REEMPLAZAR servicios externos con Mocks
                // Creamos un Mock para IEmailService que no hace nada (no enviará correos reales)
                var emailServiceMock = new Mock<IEmailService>();

                // Removemos el servicio real
                services.RemoveAll<IEmailService>();

                // Registramos el objeto Mock Singleton
                services.AddSingleton(emailServiceMock.Object);

                // 3. Ejecutar el Seeding (Población Inicial)
                var serviceProvider = services.BuildServiceProvider();
                using (var scope = serviceProvider.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<AppDbContext>();

                    // Asegura que la DB se cree
                    db.Database.EnsureCreated();

                    // ** LLAMADA A TU SEEDER **
                    // Esperamos (Wait) porque el ConfigureWebHost no puede ser 'async'.
                    IdentitySeed.SeedAsync(scopedServices).Wait();
                }
            });
        }
    }
}