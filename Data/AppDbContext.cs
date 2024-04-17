using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using GBC_TRAVEL_GROUP_88.Areas.BookingManagement.Models;
using System.Reflection.Emit;
namespace GBC_TRAVEL_GROUP_88.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Flight> Flights { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<RentalCar> RentalCars { get; set; }
        public DbSet<FlightBooking> FlightBookings { get; set; }
        public DbSet<HotelBooking> HotelBookings { get; set; }
        public DbSet<RentalCarBooking> RentalCarBookings { get; set; }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.HasDefaultSchema("Identity");

            builder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable(name: "User");
            });

            builder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable(name: "Role");
            });

            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable(name: "UserRoles");
            });

            builder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable(name: "UserClaims");
            });

            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable(name: "UserLogins");
            });

            builder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable(name: "RoleClaims");
            });

            builder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable(name: "UserTokens");
            });

            builder.Entity<Hotel>()
                .Property(h => h.Price)
                .HasColumnType("decimal(18,2)");
            builder.Entity<Flight>()
                .Property(h => h.Price)
                .HasColumnType("decimal(18,2)");
            builder.Entity<RentalCar>()
                .Property(h => h.Price)
                .HasColumnType("decimal(18,2)");
        }
    }
}
