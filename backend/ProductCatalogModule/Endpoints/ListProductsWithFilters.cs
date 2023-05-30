using System.Security.Claims;

namespace Backend.ProductCatalogModule;

public class ListProductsWithFilters
{
    private readonly IProductRepository _productRepository;
    private readonly IAuthorizationService _authorizationService;
    private readonly ClaimsPrincipal _user;

    public ListProductsWithFilters(IProductRepository productRepository, IAuthorizationService authorizationService, ClaimsPrincipal user)
    {
        _productRepository = productRepository;
        _authorizationService = authorizationService;
        _user = user;
    }

    public async Task<IEnumerable<Product>> ExecuteAsync(string? name, decimal? minPrice, decimal? maxPrice, bool? onlyInStock)
    {
        _authorizationService.Authorize(Permissions.ListProductsWithFilters, _user);
        return await _productRepository.ListAsync(name, minPrice, maxPrice, onlyInStock);
    }
}