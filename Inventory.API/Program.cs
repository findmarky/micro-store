var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHealthChecks();

var app = builder.Build();
app.MapHealthChecks("/health");

var random = new Random();
app.MapGet("/inventory", () =>
{
    var inventories = Enumerable.Range(1, 5).Select(index =>
        new Inventory(Guid.NewGuid(), $"Item {index}", random.Next(1, 50))
    ).ToArray();

    return inventories;
});

app.Run();

internal record Inventory(Guid Id, string Item, int Quantity);
