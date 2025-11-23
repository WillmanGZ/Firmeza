using Microsoft.EntityFrameworkCore;

namespace AdminManager.Web.Extensions
{
    public static class MigrationExtensions
    {
        public static void ApplyMigrations<TDbContext>(this IApplicationBuilder app) where TDbContext : DbContext
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<TDbContext>>();
                var context = services.GetRequiredService<TDbContext>();

                try
                {
                    logger.LogInformation("--> Intentando aplicar migraciones...");

                    // Verificamos si hay migraciones pendientes
                    if (context.Database.GetPendingMigrations().Any())
                    {
                        // INTENTO DE CONEXIÓN CON RETRY (Por si la BD aún está iniciando)
                        // Esto es vital en Docker Compose
                        var retryCount = 0;
                        var maxRetries = 5;

                        while (retryCount < maxRetries)
                        {
                            try
                            {
                                if (context.Database.CanConnect())
                                {
                                    context.Database.Migrate();
                                    logger.LogInformation("--> Migraciones aplicadas correctamente.");
                                    return; // Salimos si tuvo éxito
                                }
                            }
                            catch (Exception)
                            {
                                retryCount++;
                                logger.LogWarning($"--> La base de datos no está lista. Reintentando {retryCount}/{maxRetries} en 2 segundos...");
                                System.Threading.Thread.Sleep(2000); // Espera 2 segundos
                            }
                        }

                        // Si llegamos aquí, intentamos una última vez para que lance la excepción real si falla
                        context.Database.Migrate();
                    }
                    else
                    {
                        logger.LogInformation("--> No hay migraciones pendientes.");
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "--> ERROR CRÍTICO: No se pudieron aplicar las migraciones.");
                    // Opcional: throw; // Si quieres que la app se detenga si no hay BD
                }
            }
        }
    }
}