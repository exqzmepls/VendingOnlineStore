using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        var r = Database.EnsureCreated();
    }

    public DbSet<BagMachine> BagMachines { get; set; } = null!;

    public DbSet<BagItem> BagItems { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        var bagMachines = new BagMachine[]
        {
            new BagMachine
            {
                Id = Guid.NewGuid(),
                ExternalId = "m1"
            },
            new BagMachine
            {
                Id = Guid.NewGuid(),
                ExternalId = "m2"
            },
        };
        builder.Entity<BagMachine>().HasData(bagMachines);

        var bagItems = new BagItem[]
        {
            new BagItem
            {
                Id = Guid.NewGuid(),
                ExternalId = "i1",
                Count = 1,
                BagMachineId = bagMachines[0].Id
            },
            new BagItem
            {
                Id = Guid.NewGuid(),
                ExternalId = "i2",
                Count = 3,
                BagMachineId = bagMachines[0].Id
            },
            new BagItem
            {
                Id = Guid.NewGuid(),
                ExternalId = "i3",
                Count = 2,
                BagMachineId = bagMachines[0].Id
            },
            new BagItem
            {
                Id = Guid.NewGuid(),
                ExternalId = "i1",
                Count = 5,
                BagMachineId = bagMachines[1].Id
            },
            new BagItem
            {
                Id = Guid.NewGuid(),
                ExternalId = "i2",
                Count = 2,
                BagMachineId = bagMachines[1].Id
            }
        };
        builder.Entity<BagItem>().HasData(bagItems);
    }
}
