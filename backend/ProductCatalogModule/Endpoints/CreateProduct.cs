using System.Security.Claims;
namespace Backend.ProductCatalogModule
{
    using System;
    using System.Threading.Tasks;

    public class CreateProduct
    {
        private readonly IProductRepository _productRepository;
        private readonly IAuthorizationService _authorizationService;
        private readonly ClaimsPrincipal _user;

        public CreateProduct(IProductRepository productRepository, IAuthorizationService authorizationService, ClaimsPrincipal user)
        {
            _productRepository = productRepository;
            _authorizationService = authorizationService;
            _user = user;
        }

        public async Task<Product> ExecuteAsync(Product product)
        {
            _authorizationService.Authorize(Permissions.CreateProduct, _user);
            product.Id = Guid.NewGuid();
            await _productRepository.AddAsync(product);
            return product;
        }
    }
}