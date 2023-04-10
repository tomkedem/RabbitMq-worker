using Microsoft.EntityFrameworkCore;
using ServiceWorkerRabbitMqTest.Data;
using ServiceWorkerRabbitMqTest.Extensions;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddApplicationServices(builder.Configuration);
      
        builder.Services.AddHostedService<BackgroundWorkerService>();
      

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

       
        app.UseHttpsRedirection();   

        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<DataContext>();
            context.Database.EnsureCreated();
            
            await Seed.SeedData(context);

        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred during migration");
        }
        app.Run();
    }
}