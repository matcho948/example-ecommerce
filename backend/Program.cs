using System.Security.Claims;
using Backend;
using Backend.Adapters;
using Backend.ImageUploadModule;
using Backend.Implementations;
using Backend.InventoryModule;
using Backend.MockImplementations;
using Backend.Model;
using Backend.ProductCatalogModule;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseKestrel();
IConfiguration config = builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();
builder.Services.AddSingleton(config);
builder.Services.AddMicrosoftIdentityWebApiAuthentication(builder.Configuration);
builder.Services.AddAuthorization();
// TODO
builder.Services.AddDbContext<BackendDbContext>(opt => opt.UseInMemoryDatabase("mock"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();
builder.Services.AddScoped<IImageRepository, ImageRepository>();
var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", [Authorize] (ClaimsPrincipal user) => $"Hello World, {user.IsInRole("Admin")}");

AuthorizationService authorizationService = new(new Dictionary<string, IEnumerable<Permission>>
{
    ["Admin"] = new[] { Permission.GetProduct, Permission.ListProducts, Permission.ListProductsWithFilters, Permission.CreateProduct, Permission.UpdateProduct, Permission.AddStock, Permission.RemoveStock, Permission.UploadImage },
    ["User"] = new[] { Permission.GetProduct, Permission.ListProducts, Permission.ListProductsWithFilters }
});

new ProductCatalogModule()
    // TODO replace with AD Auth
    .AddModule(new AuthorizationAdapters(authorizationService.Authorize))
    .ToList()
    .ForEach(endpoint => app.MapMethods(endpoint.Path, new[] { endpoint.Method.Method }, endpoint.Handler));

new InventoryModule()
    // TODO replace with AD Auth
    .AddModule(new AuthorizationAdapters(authorizationService.Authorize))
    .ToList()
    .ForEach(endpoint => app.MapMethods(endpoint.Path, new[] { endpoint.Method.Method }, endpoint.Handler));

new ImageUploadModule()
    // TODO replace with AD Auth and Blob Storage implementation
    .AddModule(new AuthorizationAdapters(authorizationService.Authorize), new MockImageUploadService())
    .ToList()
    .ForEach(endpoint => app.MapMethods(endpoint.Path, new[] { endpoint.Method.Method }, endpoint.Handler));

app.Run();
