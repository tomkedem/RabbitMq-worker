using Microsoft.EntityFrameworkCore;
using ServiceWorkerRabbitMqTest.Entities;

namespace ServiceWorkerRabbitMqTest.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Users> Users { get; set; }
        public DbSet<Items> Items { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Items>()
                .HasOne(i => i.Users)
                .WithMany(u => u.Items)
                .HasForeignKey(i => i.UserId);
        }

    }
}
