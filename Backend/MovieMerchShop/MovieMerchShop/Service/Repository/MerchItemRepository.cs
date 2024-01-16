using Microsoft.EntityFrameworkCore;
using MovieMerchShop.Data;
using MovieMerchShop.Model;

namespace MovieMerchShop.Service.Repository;

public class MerchItemRepository : IMerchItemRepository
{
    private readonly AppDbContext _dbContext;

    public MerchItemRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<IEnumerable<MerchItem>> GetAllItemsAsync()
    {
        return await _dbContext.MerchItems.ToListAsync();
    }

    public async Task<MerchItem?> GetItemByIdAsync(Guid itemId)
    {
        return await _dbContext.MerchItems.FindAsync(itemId);
    }

    public async Task UpdateItemAsync(MerchItem itemToUpdate)
    {
        _dbContext.MerchItems.Update(itemToUpdate);
        await _dbContext.SaveChangesAsync();
    }
}