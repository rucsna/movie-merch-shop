using MovieMerchShop.Model;

namespace MovieMerchShop.Service.Repository;

public interface IOrderRepository
{
    Task<Order> CreateNewOrderAsync(Order newOrder);
    Task<IEnumerable<Order>> GetAllAsync();
    Task<IEnumerable<Order>> GetByUserIdAsync(Guid userId);
    Task DeleteOrderAsync(Order orderToDelete);
}