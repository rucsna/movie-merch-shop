using MovieMerchShop.Model;

namespace MovieMerchShop.Contracts;

public record OrderRequest(
    string UserEmail,
    List<Guid> OrderedItemIds,
    decimal OrderSum
);