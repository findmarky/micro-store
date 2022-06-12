var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks();
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();
app.MapHealthChecks("/health");
app.Logger.LogInformation("The Account.API has started");

var random = new Random();
app.MapGet("/account", (ILogger<Program> logger) =>
{
    logger.LogInformation("Fetching accounts");

    var accounts = Enumerable.Range(1, 5).Select(index =>
        new Account(Guid.NewGuid(), $"Account {index}", random.Next(1, 2000))
    ).ToArray();

    return accounts;
});

app.Run();

internal record Account(Guid Id, string Name, decimal Balance);
