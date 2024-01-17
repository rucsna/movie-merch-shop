using System.Collections.Specialized;
using System.Data.Entity;
using MovieMerchShop.Data;
using MovieMerchShop.Model;

namespace MovieMerchShop.Service.Repository;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _dbContext;

    public OrderRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Order> CreateNewOrderAsync(Order newOrder)
    {
        await _dbContext.Orders.AddAsync(newOrder);
        await _dbContext.SaveChangesAsync();

        return newOrder;
    }

    public async Task<IEnumerable<Order>> GetAllAsync()
    {
        return await _dbContext.Orders.ToListAsync();
    }
    
    public async Task<Order?> GetOrderByIdAsync(Guid orderId)
    {
        return await _dbContext.Orders.FindAsync(orderId);
    }

    public async Task<IEnumerable<Order>> GetByUserIdAsync(string userId)
    {
        return await _dbContext.Orders.Where(order => order.UserId == userId).ToListAsync();
    }

    public async Task DeleteOrderAsync(Order orderToDelete)
    {
        _dbContext.Orders.Remove(orderToDelete);
        await _dbContext.SaveChangesAsync();
    }
}