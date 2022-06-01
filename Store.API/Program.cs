var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddHealthChecks();
builder.Services.AddHttpClient();

builder.Services.AddSingleton(configuration.GetSection("Services").Get<ServicesConfig>());

var app = builder.Build();
app.MapHealthChecks("/health");

app.MapGet("/hello", async (IHttpClientFactory httpClientFactory, ServicesConfig servicesConfig) =>
{
    var accountClient = httpClientFactory.CreateClient();
    accountClient.BaseAddress = new Uri(servicesConfig.AccountServiceUrl);

    var inventoryClient = httpClientFactory.CreateClient();
    inventoryClient.BaseAddress = new Uri(servicesConfig.InventoryServiceUrl);

    var accountsResponseMessage = await accountClient.GetAsync("/account");
    var inventoriesResponseMessage = await inventoryClient.GetAsync("/inventory");

    var accounts = accountsResponseMessage.IsSuccessStatusCode ? await accountsResponseMessage.Content.ReadFromJsonAsync<List<Account>>() : null;
    var inventories = inventoriesResponseMessage.IsSuccessStatusCode ? await inventoriesResponseMessage.Content.ReadFromJsonAsync<List<Inventory>>() : null;

    var response = new
    {
        accounts,
        inventories
    };

    return Results.Ok(response);
});

app.Run();


internal record Inventory(Guid id, string item, int quantity);
internal record Account(Guid id, string name, decimal balance);

public class ServicesConfig
{
    public string AccountServiceUrl { get; set; }

    public string InventoryServiceUrl { get; set; }
}