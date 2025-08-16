using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RakbnyMa_aak.Models;
using System.Reflection.Emit;

namespace RakbnyMa_aak.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Payment> Payments { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<WalletTransaction> WalletTransactions { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Governorate> Governorates { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<TripTracking> TripTrackings { get; set; }
        public DbSet<Notification> Notifications { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // 🟢 Global Query Filters (Soft Delete)
            builder.Entity<Trip>().HasQueryFilter(t => !t.IsDeleted);
            builder.Entity<Booking>().HasQueryFilter(t => !t.IsDeleted);
            builder.Entity<Rating>().HasQueryFilter(t => !t.IsDeleted);
            builder.Entity<Message>().HasQueryFilter(t => !t.IsDeleted);
            builder.Entity<Governorate>().HasQueryFilter(g => !g.IsDeleted);
            builder.Entity<City>().HasQueryFilter(c => !c.IsDeleted);
            builder.Entity<Message>().HasQueryFilter(m => !m.IsDeleted);
            builder.Entity<Notification>().HasQueryFilter(n => !n.IsDeleted);

            builder.Entity<Rating>()
                .HasIndex(r => new { r.RaterId, r.RatedId, r.TripId })
                .IsUnique();

            builder.Entity<Driver>()
                .HasIndex(d => d.NationalId)
                .IsUnique();

            builder.Entity<City>()
                .HasIndex(c => new { c.Name, c.GovernorateId })
                .IsUnique();

            builder.Entity<Governorate>()
                .HasIndex(g => g.Name)
                .IsUnique();

            builder.Entity<Rating>()
                .HasOne(r => r.Rater)
                .WithMany(u => u.RatingsGiven)
                .HasForeignKey(r => r.RaterId)
                .OnDelete(DeleteBehavior.Restrict); 

            builder.Entity<Rating>()
                .HasOne(r => r.Rated)
                .WithMany(u => u.RatingsReceived)
                .HasForeignKey(r => r.RatedId)
                .OnDelete(DeleteBehavior.Restrict);


            //Enums To String Conversion
            builder.Entity<ApplicationUser>()
               .Property(u => u.UserType)
               .HasConversion<string>();

            builder.Entity<Driver>()
                .Property(d => d.CarType)
                .HasConversion<string>();

            builder.Entity<Driver>()
                .Property(d => d.CarColor)
                .HasConversion<string>();

            builder.Entity<Trip>()
                .Property(t => t.TripStatus)
                .HasConversion<string>();

            builder.Entity<Booking>()
                .Property(b => b.RequestStatus)
                .HasConversion<string>();

            // Configure Payment relationships
            builder.Entity<Payment>()
                .HasOne(p => p.User)
                .WithMany()
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Payment>()
                .HasOne(p => p.Booking)
                .WithOne(b => b.Payment)
                .HasForeignKey<Payment>(p => p.BookingId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Wallet Transactions
            builder.Entity<WalletTransaction>()
                .HasOne(t => t.Wallet)
                .WithMany(w => w.Transactions)
                .HasForeignKey(t => t.WalletUserId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
