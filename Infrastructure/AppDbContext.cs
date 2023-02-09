using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public sealed class AppDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
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
}