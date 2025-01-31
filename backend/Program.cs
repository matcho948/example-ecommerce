using Backend;
using Backend.Adapters;
using Backend.ImageUploadModule;
using Backend.Implementations;
using Backend.InventoryModule;
using Backend.MockImplementations;
using Backend.ProductCatalogModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseIISIntegration();
IConfiguration config = builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();
builder.Services.AddSingleton(config);
builder.Services.AddDbContext<BackendDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// TODO
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IImageStorageService, ImageUploadService>();
builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();
builder.Services.AddScoped<IImageRepository, ImageRepository>();
builder.Services.AddMicrosoftIdentityWebApiAuthentication(builder.Configuration);
builder.Services.AddAuthorization();
var app = builder.Build();

app.UseHttpsRedirection();

// When implementing Auth, uncomment this line:
app.MapGet("/", [Authorize] () => "Hello World!");
// and comment this one

MockAuthorizationService mockAuthorizationService = new();

new ProductCatalogModule()
    // TODO replace with AD Auth
    .AddModule(new AuthorizationAdapters(mockAuthorizationService.Authorize))
    .ToList()
    .ForEach(endpoint => app.MapMethods(endpoint.Path, new[] { endpoint.Method.Method }, endpoint.Handler));

new InventoryModule()
    // TODO replace with AD Auth
    .AddModule(new AuthorizationAdapters(mockAuthorizationService.Authorize))
    .ToList()
    .ForEach(endpoint => app.MapMethods(endpoint.Path, new[] { endpoint.Method.Method }, endpoint.Handler));

new ImageUploadModule()
    // TODO replace with AD Auth and Blob Storage implementation
    .AddModule(new AuthorizationAdapters(mockAuthorizationService.Authorize), new ImageUploadService(builder.Configuration))
    .ToList()
    .ForEach(endpoint => app.MapMethods(endpoint.Path, new[] { endpoint.Method.Method }, endpoint.Handler));

app.UseAuthentication();
app.UseAuthorization();
app.Run();
