using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        var r = Database.EnsureCreated();
    }

    public DbSet<BagSection> BagSections { get; set; } = null!;

    public DbSet<BagContent> BagContents { get; set; } = null!;

    public DbSet<Order> Orders { get; set; } = null!;

    public DbSet<OrderPickupPoint> OrderPickupPoints { get; set; } = null!;

    public DbSet<OrderContent> OrderContents { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        var bagMachines = new BagSection[]
        {
            new BagSection
            {
                Id = Guid.NewGuid(),
                PickupPointId = "m1"
            },
            new BagSection
            {
                Id = Guid.NewGuid(),
                PickupPointId = "m2"
            },
        };
        builder.Entity<BagSection>().HasData(bagMachines);

        var bagItems = new BagContent[]
        {
            new BagContent
            {
                Id = Guid.NewGuid(),
                ItemId = "i1",
                Count = 1,
                BagSectionId = bagMachines[0].Id
            },
            new BagContent
            {
                Id = Guid.NewGuid(),
                ItemId = "i2",
                Count = 3,
                BagSectionId = bagMachines[0].Id
            },
            new BagContent
            {
                Id = Guid.NewGuid(),
                ItemId = "i3",
                Count = 2,
                BagSectionId = bagMachines[0].Id
            },
            new BagContent
            {
                Id = Guid.NewGuid(),
                ItemId = "i1",
                Count = 5,
                BagSectionId = bagMachines[1].Id
            },
            new BagContent
            {
                Id = Guid.NewGuid(),
                ItemId = "i2",
                Count = 2,
                BagSectionId = bagMachines[1].Id
            }
        };
        builder.Entity<BagContent>().HasData(bagItems);
    }
}