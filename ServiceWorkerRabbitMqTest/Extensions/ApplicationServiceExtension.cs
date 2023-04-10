using Microsoft.EntityFrameworkCore;
using ServiceWorkerRabbitMqTest.Data;


namespace ServiceWorkerRabbitMqTest.Extensions
{
    public static class ApplicationServiceExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration? config)
        {

            // Register db connetion to DI
            string connString = config.GetConnectionString("DefaultConnection");
            services.AddDbContext<DataContext>(options =>
            {
                //options.UseSqlServer(connString);
                options.UseNpgsql(connString);
            });

            return services;
        }
    }
}
