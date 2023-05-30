using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Backend.ProductCatalogModule;

public class ProductCatalogModule : IModule<IAuthorizationService>
{
    public ApiEndpoint[] AddModule(IAuthorizationService authorizationService)
    {
        return new[]
        {
            new ApiEndpoint("/products", (string? name, decimal? minPrice, decimal? maxPrice, bool? onlyInStock, IProductRepository productRepository, ClaimsPrincipal user) => {
                if (name is not null || minPrice is not null || maxPrice is not null || onlyInStock is not null)
                {
                    return new ListProductsWithFilters(productRepository, authorizationService, user).ExecuteAsync(name, minPrice, maxPrice, onlyInStock);
                }
                else
                {
                    return new ListProducts(productRepository, authorizationService, user).ExecuteAsync();
                }
            }),
            new ApiEndpoint("/products/{id}", (Guid id, IProductRepository productRepository, ClaimsPrincipal user) => new GetProduct(productRepository, authorizationService, user).ExecuteAsync(id)),
            new ApiEndpoint("/products", (Product product, IProductRepository productRepository, ClaimsPrincipal user) => new CreateProduct(productRepository, authorizationService, user).ExecuteAsync(product), HttpMethod.Post),
            new ApiEndpoint("/products/{id}", (Guid id, Product product, IProductRepository productRepository, ClaimsPrincipal user) => new UpdateProduct(productRepository, authorizationService, user).ExecuteAsync(id, product), HttpMethod.Put),
        };
    }
}