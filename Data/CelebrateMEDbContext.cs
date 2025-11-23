using Microsoft.EntityFrameworkCore;
using CelebrateME.Models;

public class CelebrateMEDbContext : DbContext
{
    public CelebrateMEDbContext(DbContextOptions<CelebrateMEDbContext> options) : base(options)
    {
    }

    public DbSet<Book> Books { get; set; }
    public DbSet<Card> Cards { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Gift> Gifts { get; set; }

    // new DbSets
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<AccessCode> AccessCodes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // configure simple relationships
        modelBuilder.Entity<Product>()
            .HasOne(p => p.Category)
            .WithMany()
            .HasForeignKey(p => p.CategoryId);

        modelBuilder.Entity<Order>()
            .HasMany(o => o.Items)
            .WithOne(i => i.Order)
            .HasForeignKey(i => i.OrderId);

        modelBuilder.Entity<OrderItem>()
            .HasOne(i => i.Product)
            .WithMany()
            .HasForeignKey(i => i.ProductId);
    }
}