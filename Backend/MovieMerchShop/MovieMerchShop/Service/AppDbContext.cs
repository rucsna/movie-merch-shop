using Microsoft.EntityFrameworkCore;
using MovieMerchShop.Model;

namespace MovieMerchShop.Service;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Movie> Movies { get; set; }
    public DbSet<MerchItem> Items { get; set; }
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
}