using Microsoft.EntityFrameworkCore;
using MovieMerchShop.Model;

namespace MovieMerchShop.Service;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Movie> Movies { get; set; }
    public DbSet<MerchItem> MerchItems { get; set; }
    // public DbSet<Mug> Mugs { get; set; }
    // public DbSet<Shirt> Shirts { get; set; }
    // public DbSet<Poster> Posters { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MerchItem>()
            .HasDiscriminator<string>("ItemType")
            .HasValue<Mug>("Mug")
            .HasValue<Poster>("Poster")
            .HasValue<Shirt>("Shirt");
    }
}