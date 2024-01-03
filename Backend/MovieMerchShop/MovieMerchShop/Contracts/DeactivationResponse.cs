namespace MovieMerchShop.Contracts;

public record DeactivationResponse(
    string UserName,
    string Message = "Account successfully deactivated."
    );