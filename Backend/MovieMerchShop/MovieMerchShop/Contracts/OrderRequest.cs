using MovieMerchShop.Model;

namespace MovieMerchShop.Contracts;

public record OrderRequest(
    Guid UserId,
    List<Guid> OrderedItemIds,
    decimal OrderSum
);