using System.Security.Claims;

namespace Backend.ProductCatalogModule;

public class GetProduct
{
    private readonly IProductRepository _productRepository;
    private readonly IAuthorizationService _authorizationService;
    private readonly ClaimsPrincipal _user;

    public GetProduct(IProductRepository productRepository, IAuthorizationService authorizationService, ClaimsPrincipal user)
    {
        _productRepository = productRepository;
        _authorizationService = authorizationService;
        _user = user;
    }

    public async Task<Product> ExecuteAsync(Guid id)
    {
        _authorizationService.Authorize(Permissions.GetProduct, _user);
        return await _productRepository.GetAsync(id);
    }
}