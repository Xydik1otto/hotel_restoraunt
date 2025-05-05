using hotel_restoraunt.Models;
using Microsoft.EntityFrameworkCore;

namespace hotel_restoraunt.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(
                "server=localhost;database=hotel_restoraunt_db;user=root;password=;",
                new MySqlServerVersion(new Version(10, 4, 24))
            );
        }
    }
}