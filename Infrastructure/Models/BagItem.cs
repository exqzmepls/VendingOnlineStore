using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Models;

public class BagItem
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public string ExternalId { get; set; } = null!;

    public int Count { get; set; }

    public Guid BagMachineId { get; set; }

    public BagMachine? BagMachine { get; set; }
}
