using Microsoft.EntityFrameworkCore;
using MovieMerchShop.Model;

namespace MovieMerchShop.Data;

public class AppDbContext : DbContext
{
    public DbSet<Order> Orders { get; set; }
    public DbSet<Movie> Movies { get; set; }

    public DbSet<MerchItem> MerchItems { get; set; }
    public DbSet<Mug> Mugs { get; set; }
    public DbSet<Shirt> Shirts { get; set; }
    public DbSet<Poster> Posters { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<MovieGenre> MovieGenres { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Shirt>()
            .Property(e => e.Color)
            .HasConversion<string>();

        modelBuilder.Entity<Shirt>()
            .Property(e => e.Size)
            .HasConversion<string>();

          modelBuilder.Entity<Mug>()
                    .Property(e => e.Color)
                    .HasConversion<string>();

          modelBuilder.Entity<Poster>()
              .Property(e => e.Size)
              .HasConversion<string>();

          modelBuilder.Entity<Poster>()
              .Property(e => e.Material)
              .HasConversion<string>();

        
        
        // modelBuilder.Entity<Shirt>()
        //     .Property(e => e.Type)
        //     .HasConversion<string>();
        

        base.OnModelCreating(modelBuilder);
    }
}