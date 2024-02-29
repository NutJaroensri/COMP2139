using Microsoft.EntityFrameworkCore;
using GBC_Travel_Group_83.Models;
namespace GBC_Travel_Group_83.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {
        
        }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<FlightBooking> FlightBookings { get; set; }
        public DbSet<HotelBooking> HotelBookings { get; set; }
        public DbSet<RentalBooking> RentalBookings { get; set;}


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Specify column types for properties
            modelBuilder.Entity<Hotel>()
                .Property(h => h.Price)
                .HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Flight>()
                .Property(h => h.Price)
                .HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Rental>()
                .Property(h => h.Price)
                .HasColumnType("decimal(18,2)");
           

        }
    }
}
