var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks();
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();
app.MapHealthChecks("/health");
app.Logger.LogInformation("The Inventory.API has started");

var random = new Random();
app.MapGet("/inventory", (ILogger<Program> logger) =>
{
    logger.LogInformation("Fetching inventories");

    var inventories = Enumerable.Range(1, 5).Select(index =>
        new Inventory(Guid.NewGuid(), $"Inventory item {index}", random.Next(0, 100))
    ).ToArray();

    return inventories;
});

app.Run();

internal record Inventory(Guid Id, string Item, int Quantity);
