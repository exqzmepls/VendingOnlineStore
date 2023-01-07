using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Models;

public class BagMachine
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public string ExternalId { get; set; } = null!;

    public IEnumerable<BagItem>? BagItems { get; set; }
}
