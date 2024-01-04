using MovieMerchShop.Model;

namespace MovieMerchShop.Service.Authentication;

public interface ITokenService
{
    string CreateToken(ApplicationUser user, string role);
}