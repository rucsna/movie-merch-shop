namespace MovieMerchShop.Service.Authentication;

public record AuthRequest(
    string Email,
    string Password
    );