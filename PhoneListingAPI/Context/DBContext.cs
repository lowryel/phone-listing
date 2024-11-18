using System.Reflection.Emit;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using PhoneListingAPI.Models;

namespace MobilePhoneListing.DBContext;
public class MobilePhoneDbContext : DbContext
{
    public MobilePhoneDbContext(DbContextOptions<MobilePhoneDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Listing> Listings { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<ListingCategory> ListingCategories { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Listing>()
            .Property(l => l.Price)
            .HasColumnType("decimal(18, 2)"); 

        // Configure many-to-many relationship
        modelBuilder.Entity<ListingCategory>()
            .HasKey(lc => new { lc.ListingId, lc.CategoryId });

        modelBuilder.Entity<ListingCategory>()
            .HasOne(lc => lc.Listing)
            .WithMany(l => l.Categories)
            .HasForeignKey(lc => lc.ListingId);

        modelBuilder.Entity<ListingCategory>()
            .HasOne(lc => lc.Category)
            .WithMany(c => c.Listings)
            .HasForeignKey(lc => lc.CategoryId);

        // modelBuilder.Entity<User>()
        //     .HasQueryFilter(user => user.IsDeleted == true)
        //     .HasMany(l => l.Listings);

        // modelBuilder.Entity<User>()
        //     .HasIndex(u => u.Username)
        //     .IsUnique();


        Guid userId1 = Guid.NewGuid();
        Guid userId2 = Guid.NewGuid();

        modelBuilder.Entity<User>().HasData(
            new User
            {
                UserId = userId1,
                Username = "JohnDoe",
                Email = "john.doe@example.com",
                PasswordHash = "hashed_password_123", // Replace with actual hash
                Active = true,
                IsDeleted = false
            },
            new User
            {
                UserId = userId2,
                Username = "JaneDoe",
                Email = "jane.doe@example.com",
                PasswordHash = "hashed_password_456", // Replace with actual hash
                Active = true,
                IsDeleted = false
            }
        );

        modelBuilder.Entity<Listing>().HasData(
            new Listing
            {
                ListingId = Guid.NewGuid(),
                Title = "iPhone 14 Pro",
                Description = "Brand new iPhone 14 Pro for sale.",
                Price = 999.99m,
                CreatedAt = DateTime.Now,
                UserId = userId1
            },
            new Listing
            {
                ListingId = Guid.NewGuid(),
                Title = "Samsung Galaxy S23",
                Description = "Almost new Samsung Galaxy S23, great condition.",
                Price = 850.00m,
                CreatedAt = DateTime.Now,
                UserId = userId2
            }
        );


    }
}
