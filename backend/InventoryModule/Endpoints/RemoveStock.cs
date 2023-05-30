using System.Security.Claims;

namespace Backend.InventoryModule;

public class RemoveStock
{
    private readonly IInventoryRepository _inventoryRepository;
    private readonly IAuthorizationService _authorizationService;
    private readonly ClaimsPrincipal _user;

    public RemoveStock(IInventoryRepository inventoryRepository, IAuthorizationService authorizationService, ClaimsPrincipal user)
    {
        _inventoryRepository = inventoryRepository;
        _authorizationService = authorizationService;
        _user = user;
    }

    public async Task<int> ExecuteAsync(Guid productId, int quantity)
    {
        _authorizationService.Authorize(Permissions.RemoveStock, _user);
        return await _inventoryRepository.RemoveStockAsync(productId, quantity);
    }
}