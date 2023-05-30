using System.Security.Claims;

namespace Backend.ProductCatalogModule;

public class ListProducts
{
    private readonly IProductRepository _productRepository;
    private readonly IAuthorizationService _authorizationService;
    private readonly ClaimsPrincipal _user;

    public ListProducts(IProductRepository productRepository, IAuthorizationService authorizationService, ClaimsPrincipal user)
    {
        _productRepository = productRepository;
        _authorizationService = authorizationService;
        _user = user;
    }

    public async Task<IEnumerable<Product>> ExecuteAsync()
    {
        _authorizationService.Authorize(Permissions.ListProducts, _user);
        return await _productRepository.ListAsync();
    }
}