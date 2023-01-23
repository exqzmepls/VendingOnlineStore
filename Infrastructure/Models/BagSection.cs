using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Models;

public class BagSection
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public string PickupPointId { get; set; } = null!;

    public IReadOnlyCollection<BagContent>? BagContents { get; set; }
}