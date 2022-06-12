var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddHealthChecks();
builder.Services.AddHttpClient();
builder.Services.AddSingleton(configuration.GetSection("Services").Get<ServicesConfig>());

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();

app.MapHealthChecks("/health");
app.Logger.LogInformation("The Store.API has started");

app.MapGet("/store", async (IHttpClientFactory httpClientFactory, ILogger<Program> logger, ServicesConfig servicesConfig) =>
{
    async Task<Account[]?> GetAllAccounts()
    {
        try
        {
            var accountClient = httpClientFactory.CreateClient();
            accountClient.BaseAddress = new Uri(servicesConfig.AccountServiceUrl);

            var accountsResponseMessage = await accountClient.GetAsync("/account");

            return accountsResponseMessage.IsSuccessStatusCode ? await accountsResponseMessage.Content.ReadFromJsonAsync<Account[]>() : null;
        }
        catch (Exception exception)
        {
            logger.LogError(exception, $"An error occurred when fetching the accounts from {servicesConfig.AccountServiceUrl}");
            return null;
        }
    }

    async Task<Inventory[]?> GetAllInventory()
    {
        try
        {
            var inventoryClient = httpClientFactory.CreateClient();
            inventoryClient.BaseAddress = new Uri(servicesConfig.InventoryServiceUrl);

            var inventoriesResponseMessage = await inventoryClient.GetAsync("/inventory");

            return inventoriesResponseMessage.IsSuccessStatusCode ? await inventoriesResponseMessage.Content.ReadFromJsonAsync<Inventory[]>() : null;
        }
        catch (Exception exception)
        {
            logger.LogError(exception, $"An error occurred when fetching inventories from {servicesConfig.InventoryServiceUrl}");
            return null;
        }
    }

    var accounts = await GetAllAccounts();
    var inventories = await GetAllInventory();

    if (accounts == null && inventories == null)
    {
        return Results.Problem("Failed to retrieve account and inventory data.");
    }

    var storeData = new Store(
        accounts ?? Array.Empty<Account>(), 
        inventories ?? Array.Empty<Inventory>());

    return Results.Ok(storeData);
});

app.Run();

internal record Store(Account[] Accounts, Inventory[] Inventories);

internal record Inventory(Guid Id, string Item, int Quantity);

internal record Account(Guid Id, string Name, decimal Balance);

internal record ServicesConfig
{
    public string AccountServiceUrl { get; set; } = null!;
    public string InventoryServiceUrl { get; set; } = null!;
}
