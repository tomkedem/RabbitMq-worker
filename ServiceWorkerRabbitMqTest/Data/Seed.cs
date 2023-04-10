using Microsoft.EntityFrameworkCore;
using ServiceWorkerRabbitMqTest.Entities;
using System.Text.Json;

namespace ServiceWorkerRabbitMqTest.Data
{
    public class Seed
    {
        public static async Task SeedData(DataContext context)
        {
            if (await context.Users.AnyAsync()) return;

            var usersData = await System.IO.File.ReadAllTextAsync("Data/UsersSeedData.json");
            var users = JsonSerializer.Deserialize<List<Users>>(usersData);
            if (users == null) return;
            foreach (var user in users)
            {
                context.Users.Add(user);
            }

            await context.SaveChangesAsync();

            if (await context.Items.AnyAsync()) return;

            var itemsData = await System.IO.File.ReadAllTextAsync("Data/ItemsSeedData.json");
            var items = JsonSerializer.Deserialize<List<Items>>(itemsData);
            if (items == null) return;

            foreach (var item in items)
            {
                context.Items.Add(item);
            }

            await context.SaveChangesAsync();           

        }

    }
}
