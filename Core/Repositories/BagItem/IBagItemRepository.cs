using Core.Repositories.BagItem.Dtos;

namespace Core.Repositories.BagItem;

public interface IBagItemRepository
{
    public Task<BagItemDto?> GetOrDefaultAsync(Guid id);

    public Task<Guid?> AddAsync(NewBagItemDto newBagItem);

    public Task<BagItemDto?> UpdateAsync(Guid id, UpdatedBagItemDto updatedBagItemDto);

    public Task<bool> DeleteAsync(Guid id);
}
