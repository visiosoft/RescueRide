    using RescueRide.Models;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Reflection.Emit;

    namespace RescueRide.Data
    {
        public class AppDbContext : DbContext
        {
            public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

            // Define DbSets for your entities
            public DbSet<User> Users { get; set; }
            public DbSet<Vehicle> Vehicles { get; set; }
            public DbSet<Service> Services { get; set; }
            public DbSet<Payment> Payments { get; set; }
            public DbSet<LocationHistory> LocationHistories { get; set; }
            public DbSet<GuestUser> GuestUser { get; set; }
            public DbSet<Drivers> Drivers { get; set; }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                // Fluent API configurations if necessary

                // User configuration
                modelBuilder.Entity<User>(entity =>
                {
                    entity.HasKey(u => u.Id);
                    entity.Property(u => u.Name).IsRequired().HasMaxLength(100);
                    entity.Property(u => u.Email).IsRequired().HasMaxLength(100);
                    entity.Property(u => u.Role).IsRequired();
                });

                // Vehicle configuration
                modelBuilder.Entity<Vehicle>(entity =>
                {
                    entity.HasKey(v => v.VehicleId);
                    entity.Property(v => v.LicensePlate).IsRequired().HasMaxLength(20);
                    entity.HasOne<User>().WithMany().HasForeignKey(v => v.UserId);
                });

                // Service configuration
                modelBuilder.Entity<Service>(entity =>
                {
                    entity.HasKey(s => s.ServiceId);
                    entity.Property(s => s.ServiceType).IsRequired().HasMaxLength(50);
                    entity.Property(s => s.Status).IsRequired().HasMaxLength(20);
                    entity.HasOne<User>().WithMany().HasForeignKey(s => s.CustomerId);
                    entity.HasOne<User>().WithMany().HasForeignKey(s => s.ServiceProviderId);
                });

                // Payment configuration
                modelBuilder.Entity<Payment>(entity =>
                {
                    entity.HasKey(p => p.PaymentId);
                    entity.Property(p => p.PaymentMethod).IsRequired().HasMaxLength(50);
                    entity.HasOne<Service>().WithMany().HasForeignKey(p => p.ServiceId);
                });

                // LocationHistory configuration
                modelBuilder.Entity<LocationHistory>(entity =>
                {
                    entity.HasKey(l => l.Id);
                    entity.Property(l => l.Latitude).IsRequired();
                    entity.Property(l => l.Longitude).IsRequired();
                    entity.Property(l => l.Timestamp).IsRequired();
                    entity.HasOne<User>().WithMany().HasForeignKey(l => l.UserId);
                });

                modelBuilder.Entity<GuestUser>(entity =>
                {
                    entity.HasKey(g => g.GuestUserId);
                    entity.Property(g => g.PhoneNumber).IsRequired().HasMaxLength(20);
                    entity.Property(g => g.LicensePlate).IsRequired().HasMaxLength(20);
                });

                // ServiceProvider configuration
                modelBuilder.Entity<Drivers>(entity =>
                {
                    entity.HasKey(sp => sp.Id);
                    entity.Property(sp => sp.Name).IsRequired().HasMaxLength(100);
                    entity.Property(sp => sp.PhoneNumber).IsRequired().HasMaxLength(20);
                    entity.Property(sp => sp.Latitude).IsRequired();
                    entity.Property(sp => sp.Longitude).IsRequired();
                    entity.Property(sp => sp.IsAvailable);
                });
            }
        }
    }
