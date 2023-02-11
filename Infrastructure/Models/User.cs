using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Models;

public class User : IdentityUser<Guid>
{
    public string City { get; set; } = null!;

    public IReadOnlyCollection<Order>? Orders { get; set; }

    public IReadOnlyCollection<BagSection>? BagSections { get; set; }
}