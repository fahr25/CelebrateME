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


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>()
            .HasOne(p => p.Category)
            .WithMany()
            .HasForeignKey(p => p.CategoryId);
    }
}