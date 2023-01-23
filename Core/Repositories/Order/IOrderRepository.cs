namespace Core.Repositories.Order;

public interface IOrderRepository
{
    IQueryable<OrderDto> GetAll();

    OrderDetailsDto Update(Guid id, UpdatedOrder updatedOrder);
}