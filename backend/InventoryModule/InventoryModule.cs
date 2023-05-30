using System.Security.Claims;
namespace Backend.InventoryModule;

public class InventoryModule : IModule<IAuthorizationService>
{
    public ApiEndpoint[] AddModule(IAuthorizationService authorizationService)
    {
        return new[]{
            new ApiEndpoint("/products/{productId}/inventory", (Guid productId, IncrementDto increment, IInventoryRepository inventoryRepository, ClaimsPrincipal user) => increment.Quantity > 0
                ? new AddStock(inventoryRepository, authorizationService, user).ExecuteAsync(productId, increment.Quantity )
                : new RemoveStock(inventoryRepository, authorizationService, user).ExecuteAsync(productId, increment.Quantity ), HttpMethod.Put)
        };
    }
}