
namespace Safety.API.Extensions
{
    public static class WebApplicationExtensions
    {
        public static async Task<WebApplication> SeedDbAsync(this WebApplication app)
        {
            // To Create a scoped Request Explicitly
            using var scope = app.Services.CreateAsyncScope();

            // ServiceProvider method provide for me scoped services to choose 
            var services = scope.ServiceProvider;

            // CLR Create object from gFootContext
            var gFootContextInitializer = services.GetRequiredService<IDbInitializer>();

            // To Log Exceptions
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();

            try
            {
                // To Update Database for gFootContext
                await gFootContextInitializer.InitializeIdentityAsync();

                // To Seed Data for gFootContext
                // await gFootContextInitializer.SeedAsync();            
            }
            catch (Exception ex)
            {

                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "An error has been occured during applying the migration");
            }

            return app;
        }
    }
}
