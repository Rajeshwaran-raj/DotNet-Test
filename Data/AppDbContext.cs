using Dotnet8MySqlCrud.Models;
using Microsoft.EntityFrameworkCore;

namespace Dotnet8MySqlCrud.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(e =>
        {
            e.HasKey(p => p.Id);
            e.Property(p => p.Name).IsRequired().HasMaxLength(200);
            e.Property(p => p.Price).HasPrecision(18, 2);
            e.Property(p => p.CreatedAt)
             .HasColumnType("datetime(6)")
             .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");
        });
    }
}
