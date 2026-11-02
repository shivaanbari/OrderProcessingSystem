using InventoryService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InventoryService.Infrastructure.Persistence;

public class InventoryDbContext : DbContext
{
    public InventoryDbContext(DbContextOptions<InventoryDbContext> options)
        : base(options)
    {
    }

    public DbSet<InventoryItem> InventoryItems => Set<InventoryItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<InventoryItem>(entity =>
        {
            entity.ToTable("InventoryItems");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.ProductId)
                .IsRequired();

            entity.HasIndex(x => x.ProductId)
                .IsUnique();

            entity.Property(x => x.AvailableQuantity)
                .IsRequired();
        });
    }
}