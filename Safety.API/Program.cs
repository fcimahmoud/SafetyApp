
global using Safety.API.Extensions;
global using FirebaseAdmin;
global using Google.Apis.Auth.OAuth2;

namespace Safety.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCoreServices(builder.Configuration);
            builder.Services.AddInfraStructureServices(builder.Configuration);
            builder.Services.AddPresentationServices();
            builder.Services.AddSingleton<FirebaseInitializer>();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            
            var app = builder.Build();

            app.Services.GetRequiredService<FirebaseInitializer>();

            app.UseCustomExceptionMiddleware();
            await app.SeedDbAsync();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStaticFiles();
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }

    public class FirebaseInitializer
    {
        public FirebaseInitializer(IHostEnvironment env)
        {
            var jsonPath = Path.Combine(env.ContentRootPath, "Secrets", "safety-first-eb105-firebase-adminsdk-fbsvc-99e5a3c18e.json");

            if (FirebaseApp.DefaultInstance == null)
            {
                FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromFile(jsonPath)
                });
            }
        }
    }
}
