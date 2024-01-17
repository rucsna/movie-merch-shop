using MovieMerchShop.Model;

namespace MovieMerchShop.Service.Repository;

public interface IUserRepository
{
    Task<ApplicationUser?> GetUserByEmailAsync(string email);
    Task DeleteUserAsync(ApplicationUser userToDelete);
}