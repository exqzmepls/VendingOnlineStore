using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Models;

public class User : IdentityUser<Guid>
{
    public int CityExternalId { get; set; }

    public IReadOnlyCollection<Order>? Orders { get; set; }

    public IReadOnlyCollection<BagSection>? BagSections { get; set; }
}