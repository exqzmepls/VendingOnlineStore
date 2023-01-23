using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Models;

public class BagContent
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public string ItemId { get; set; } = null!;

    public int Count { get; set; }

    public Guid BagSectionId { get; set; }

    public BagSection? BagSection { get; set; }
}