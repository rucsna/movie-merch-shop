using MovieMerchShop.Model;

namespace MovieMerchShop.Service.Repository;

public interface IUserRepository
{
    Task<ApplicationUser?> GetUserByIdAsync(Guid userId);
}