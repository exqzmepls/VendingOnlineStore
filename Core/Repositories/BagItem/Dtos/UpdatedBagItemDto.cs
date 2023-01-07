namespace Core.Repositories.BagItem.Dtos;

public class UpdatedBagItemDto
{
    public UpdatedBagItemDto(int newCount)
    {
        NewCount = newCount;
    }

    public int NewCount { get; }
}
