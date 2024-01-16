using MovieMerchShop.Model;

namespace MovieMerchShop.Service.Repository;

public interface IMerchItemRepository
{
    Task<IEnumerable<MerchItem>> GetAllItemsAsync();
    Task<MerchItem?> GetItemByIdAsync(Guid itemId);
    Task UpdateItemAsync(MerchItem itemToUpdate);
}